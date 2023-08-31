using System;

namespace Labyrinthian.Svg
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SvgEnumFieldAttribute : Attribute
    {
        public readonly string Name;

        public SvgEnumFieldAttribute(string name)
        {
            Name = name;
        }
    }
}
