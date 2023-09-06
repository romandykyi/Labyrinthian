using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Labyrinthian.Svg
{
    public sealed class SvgWriter : IDisposable
    {
        private readonly XmlWriter _writer;
        private readonly HashSet<SvgElement> _definitions = new HashSet<SvgElement>();
        private readonly static XmlWriterSettings s_defaultSettings = new XmlWriterSettings()
        {
            Async = true
        };

        public SvgWriter(XmlWriter writer)
        {
            if (!writer.Settings.Async)
            {
                throw new InvalidOperationException("Please, set XmlWriterSettings Async to true.");
            }
            _writer = writer;
        }

        public SvgWriter(Stream stream, XmlWriterSettings? xmlSettings = null) :
            this(XmlWriter.Create(stream, xmlSettings ?? s_defaultSettings))
        { }

        public SvgWriter(TextWriter tw, XmlWriterSettings? xmlSettings = null) :
            this(XmlWriter.Create(tw, xmlSettings ?? s_defaultSettings))
        { }

        private string? SvgPropertyToString(SvgPropertyAttribute property, object propertyValue)
        {
            string? result = null;
            // string is also IEnumerable so we need to check this type before.
            if (propertyValue is string s)
            {
                result = s;
            }
            // Here we're writing SVG-collections, e.g. 'points'
            else if (propertyValue is IEnumerable enumerable)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var element in enumerable)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(property.Separator);
                    }
                    stringBuilder.Append(Convert.ToString(element, CultureInfo.InvariantCulture));
                }
                if (stringBuilder.Length > 0)
                {
                    result = stringBuilder.ToString();
                }
            }
            // Write an SVG-option, e.g. 'stroke-linecap'
            else if (propertyValue is Enum e)
            {
                return e.GetSvgOption();
            }
            // Write any other object(e.g. 'float')
            else
            {
                result = Convert.ToString(propertyValue, CultureInfo.InvariantCulture);
            }

            if (result != null && propertyValue is INeedsDefinition needsDefinitionPropery &&
                needsDefinitionPropery.Definition != null)
            {
                _definitions.Add(needsDefinitionPropery.Definition);
            }

            return result;
        }

        private async Task StartElementAsync(SvgElement element, string? ns)
        {
            var elementAttribute =
                element.GetType().GetCustomAttribute<SvgElementAttribute>() ??
                throw new InvalidOperationException($"Only svg-elements with {nameof(SvgElementAttribute)} are supported.");

            await _writer.WriteStartElementAsync(null, elementAttribute.Name, ns);

            var children = new List<SvgElement>();
            // Attributes
            foreach (var property in element.GetType().GetProperties())
            {
                var svgChildren = property.GetCustomAttribute<SvgChildrenAttribute>();
                if (svgChildren != null)
                {
                    var svgChildrenValue = property.GetValue(element);
                    if (svgChildrenValue is IEnumerable<SvgElement> enumerable)
                    {
                        children.AddRange(enumerable);
                    }
                    else if (svgChildrenValue is SvgElement child)
                    {
                        children.Add(child);
                    }

                    continue;
                }

                var svgProperty = property.GetCustomAttribute<SvgPropertyAttribute>();
                if (svgProperty == null) continue;
                var propertyValue = property.GetValue(element);
                if (propertyValue == null) continue;

                string? propertyValueStr = SvgPropertyToString(svgProperty, propertyValue);
                if (propertyValueStr == null) continue;

                if (svgProperty.Prefix != null)
                {
                    await _writer.WriteAttributeStringAsync(svgProperty.Prefix, svgProperty.Name, null, propertyValueStr);
                }
                else
                {
                    await _writer.WriteAttributeStringAsync(null, svgProperty.Name, null, propertyValueStr);
                }
            }
            // Children
            foreach (var child in children)
            {
                await StartElementAsync(child);
                await EndElementAsync();
            }
        }

        /// <summary>
        /// Start root '&lt;svg&gt;' element and the xml document.
        /// </summary>
        public async Task StartRootAsync(SvgRoot root)
        {
            _writer.WriteStartDocument();
            await StartElementAsync(root, root.Xmlns);
        }

        /// <summary>
        /// End the root element and a document.
        /// </summary>
        public async Task EndRootAsync()
        {
            if (_definitions.Count > 0)
            {
                await _writer.WriteStartElementAsync(null, "defs", null);
                foreach (var definition in _definitions)
                {
                    await StartElementAsync(definition);
                    await EndElementAsync();
                }
                await _writer.WriteEndElementAsync();
            }

            await EndElementAsync(); // end <svg>
            await _writer.WriteEndDocumentAsync();
        }

        /// <summary>
        /// Start writing an SVG-element.
        /// </summary>
        /// <param name="element">Element which will be written.</param>
        public async Task StartElementAsync(SvgElement element)
        {
            await StartElementAsync(element, null);
        }

        /// <summary>
        /// End writing an SVG-element
        /// </summary>
        public async Task EndElementAsync()
        {
            await _writer.WriteEndElementAsync();
        }

        /// <summary>
        /// Write an element with string value. Closed automatically.
        /// Use this method if you want to add '&lt;desc&gt;' or '&lt;style&gt;'.
        /// </summary>
        /// <param name="element">Element's name.</param>
        /// <param name="value">String value of the element(optional).</param>
        /// <param name="prefix">Prefix of the element(optional).</param>
        public async Task WriteStringElementAsync(string element, string? value = null, string? prefix = null)
        {
            await _writer.WriteStartElementAsync(prefix, element, null);
            if (value != null) await _writer.WriteStringAsync(value);
            await _writer.WriteEndElementAsync();
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        public void Close()
        {
            _writer.Close();
        }
    }
}
