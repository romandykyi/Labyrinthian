using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Text;

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
            if (propertyValue is IEnumerable enumerable)
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

        public void StartRoot(SvgRoot root)
        {
            _writer.WriteStartDocument();
            StartElement(root);
        }

        public void EndRoot()
        {
            _writer.WriteStartElement("defs");
            foreach (var definition in _definitions)
            {
                StartElement(definition);
                EndElement();
            }
            _writer.WriteEndElement();

            EndElement(); // end <svg>
            _writer.WriteEndDocument();
        }

        private void StartElement(SvgElement element)
        {
            var elementAttribute = 
                element.GetType().GetCustomAttribute<SvgElementAttribute>() ??
                throw new InvalidOperationException($"Only svg-elements with {nameof(SvgElementAttribute)} are supported.");

            _writer.WriteStartElement(elementAttribute.Name);

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

                _writer.WriteAttributeString(null, svgProperty.Name, propertyValueStr);
            }
            // Children
            foreach (var child in children)
            {
                StartElement(child);
                EndElement();
            }
            _writer.WriteEndElement();
        }

        public void EndElement()
        {
            _writer.WriteEndElement();
        }

        public void WriteStyle(string style)
        {
            _writer.WriteStartElement("style");
            _writer.WriteValue(style);
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
