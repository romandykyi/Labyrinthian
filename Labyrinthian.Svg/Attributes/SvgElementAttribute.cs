using System;

namespace Labyrinthian.Svg
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SvgElementAttribute : Attribute
    {
        public readonly string Name;

        public SvgElementAttribute(string name)
        {
            Name = name;
        }
    }
}
