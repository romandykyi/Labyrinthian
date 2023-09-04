using System;
using System.Reflection;

namespace Labyrinthian.Svg
{
    public static class EnumExtensions
    {
        public static string? GetSvgOption(this Enum enumValue)
        {
            var enumMemberInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (enumMemberInfo == null)
            {
                return null;
            }
            var svgEnumFieldAttribute = enumMemberInfo.GetCustomAttribute<SvgOptionAttribute>();
            if (svgEnumFieldAttribute == null)
            {
                return null;
            }
            return svgEnumFieldAttribute.Name;
        }
    }
}
