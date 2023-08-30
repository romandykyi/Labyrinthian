using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Class for manipulating SVG tags. Note, that this class doesn't validate tags
    /// and attributes so use it carefully.
    /// </summary>
    public sealed class SvgTag : IEquatable<SvgTag?>
    {
        private readonly StringBuilder _attributesBuilder;

        public readonly List<SvgTag> Children;
        public readonly string Name;
        public readonly string? Id;

        public SvgTag(string name, string? id = null)
        {
            Name = name;
            _attributesBuilder = new StringBuilder();
            Children = new List<SvgTag>();
            Id = id;

            if (Id != null) AddAttribute("id", Id);
        }

        /// <summary>
        /// Add an attribute.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="value">Value of the attribute.</param>
        public void AddAttribute(string attributeName, string value)
        {
            _attributesBuilder.Append($" {attributeName}=\"{value}\"");
        }

        /// <summary>
        /// Add an attribute.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="value">Value of the attribute.</param>
        public void AddAttribute(string attributeName, object value)
        {
            AddAttribute(attributeName, value.ToString());
        }

        /// <summary>
        /// Add a float-attribute.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="value">Value of the attribute.</param>
        public void AddAttribute(string attributeName, float value)
        {
            AddAttribute(attributeName, value.ToInvariantString());
        }

        /// <summary>
        /// Add a double-attribute.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="value">Value of the attribute.</param>
        public void AddAttribute(string attributeName, double value)
        {
            AddAttribute(attributeName, value.ToInvariantString());
        }

        /// <summary>
        /// Add a decimal-attribute.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <param name="value">Value of the attribute.</param>
        public void AddAttribute(string attributeName, decimal value)
        {
            AddAttribute(attributeName, value.ToInvariantString());
        }

        /// <summary>
        /// Convert tag and its children into SVG-string.
        /// </summary>
        /// <returns>
        /// SVG-string. Can be invalid if attributes and/or tag's name are invalid.
        /// </returns>
        public override string ToString()
        {
            StringBuilder tagBuilder = new StringBuilder();
            tagBuilder.Append($"<{Name}");
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
                tagBuilder.Append($"</{Name}>");
            }

            return tagBuilder.ToString();
        }

        /// <summary>
        /// Compare tags by ID.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is 
        /// <see cref="SvgTag"/> and has the same Id with this tag;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as SvgTag);
        }

        /// <summary>
        /// Compare tags by ID.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if tags have the same Id;
        /// <see langword="false"/> otherwise.
        /// </returns>
        public bool Equals(SvgTag? other)
        {
            return !(other is null) &&
                   Id != null && Id == other.Id;
        }

        /// <summary>
        /// HashCode that's based on the ID.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(SvgTag? left, SvgTag? right)
        {
            return EqualityComparer<SvgTag?>.Default.Equals(left, right);
        }

        public static bool operator !=(SvgTag? left, SvgTag? right)
        {
            return !(left == right);
        }
    }
}
