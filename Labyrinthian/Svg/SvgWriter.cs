using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Labyrinthian.Svg
{
    public class SvgWriter : IDisposable
    {
        private readonly XmlWriter _writer;
        private readonly HashSet<SvgElement> _definitions = new HashSet<SvgElement>();

        public SvgWriter(XmlWriter writer)
        {
            _writer = writer;
        }

        public SvgWriter(Stream stream, XmlWriterSettings? xmlSettings = null)
        {
            _writer = XmlWriter.Create(stream, xmlSettings);
        }

        public SvgWriter(TextWriter tw, XmlWriterSettings? xmlSettings = null)
        {
            _writer = XmlWriter.Create(tw, xmlSettings);
        }

        private string? SvgPropertyToString(SvgPropertyAttribute property, object propertyValue)
        {
            string? result = null;
            if (propertyValue is string s)
            {
                result = s;
            }
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
            else if (propertyValue is Enum e)
            {
                var enumMemberInfo = e.GetType().GetField(e.ToString());
                if (enumMemberInfo == null)
                {
                    return null;
                }
                var svgEnumFieldAttribute = enumMemberInfo.GetCustomAttribute<SvgEnumFieldAttribute>();
                if (svgEnumFieldAttribute == null)
                {
                    return null;
                }
                return svgEnumFieldAttribute.Name;
            }
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

        private void StartElement(SvgElement element, string? ns)
        {
            var elementAttribute =
                element.GetType().GetCustomAttribute<SvgElementAttribute>() ??
                throw new InvalidOperationException($"Only svg-elements with {nameof(SvgElementAttribute)} are supported.");

            _writer.WriteStartElement(elementAttribute.Name, ns);

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
                    _writer.WriteAttributeString(svgProperty.Prefix, svgProperty.Name, null, propertyValueStr);
                }
                else
                {
                    _writer.WriteAttributeString(svgProperty.Name, propertyValueStr);
                }
            }
            // Children
            foreach (var child in children)
            {
                StartElement(child);
                EndElement();
            }
        }

        public void StartRoot(SvgRoot root)
        {
            _writer.WriteStartDocument();
            StartElement(root, root.Xmlns);
        }

        public void EndRoot()
        {
            if (_definitions.Count > 0)
            {
                _writer.WriteStartElement("defs");
                foreach (var definition in _definitions)
                {
                    StartElement(definition);
                    EndElement();
                }
                _writer.WriteEndElement();
            }

            EndElement(); // end <svg>
            _writer.WriteEndDocument();
        }

        public void StartElement(SvgElement element)
        {
            StartElement(element, null);
        }

        public void EndElement()
        {
            _writer.WriteEndElement();
        }

        public void WriteStringElement(string element, string? value = null, string? prefix = null)
        {
            _writer.WriteStartElement(prefix, element, null);
            if (value != null) _writer.WriteValue(value);
            _writer.WriteEndElement();
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
