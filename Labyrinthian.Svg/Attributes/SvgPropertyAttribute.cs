using System;

namespace Labyrinthian.Svg
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SvgPropertyAttribute : Attribute
    {
        public readonly string Name;
        public string? Prefix { get; set; }
        public char Separator { get; set; } = ' ';

        public SvgPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
