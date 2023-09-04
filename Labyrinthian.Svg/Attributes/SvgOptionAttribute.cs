using System;

namespace Labyrinthian.Svg
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SvgOptionAttribute : Attribute
    {
        public readonly string Name;

        public SvgOptionAttribute(string name)
        {
            Name = name;
        }
    }
}
