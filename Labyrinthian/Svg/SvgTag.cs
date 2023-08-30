using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Class for manipulating SVG tags. Note, that this class doesn't validate tags
    /// and attributes so use it carefully.
    /// </summary>
    public sealed class SvgTag
    {
        private readonly string _name;
        private readonly StringBuilder _attributesBuilder;
        public readonly List<SvgTag> Children;

        public SvgTag(string name)
        {
            _name = name;
            _attributesBuilder = new StringBuilder();
            Children = new List<SvgTag>();
        }

        public void AddAttribute(string attributeName, string value)
        {
            _attributesBuilder.Append($" {attributeName}=\"{value}\"");
        }

        public void AddAttribute(string attributeName, object value)
        {
            AddAttribute(attributeName, value.ToString());
        }

        public void AddAttribute(string attributeName, float value)
        {
            AddAttribute(attributeName, value.ToInvariantString());
        }

        public override string ToString()
        {
            StringBuilder tagBuilder = new StringBuilder();
            tagBuilder.Append($"<{_name}");
            if (_attributesBuilder.Length > 0)
            {
                tagBuilder.Append($"{_attributesBuilder}");
            }
            if (Children.Count == 0)
            {
                tagBuilder.Append(" />");
            }
            else
            {
                tagBuilder.Append(">");
                foreach (SvgTag child in Children)
                {
                    tagBuilder.Append(child);
                }
                tagBuilder.Append($"</{_name}>");
            }

            return tagBuilder.ToString();
        }
    }
}
