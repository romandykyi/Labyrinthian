using System;
using System.Globalization;

namespace Labyrinthian.Svg
{
    /// <summary>
    /// Struct for defining mixed SVG-property, e.g. 'orient' for &lt;marker&gt;.
    /// </summary>
    /// <typeparam name="TEnum">Type of the <see cref="Enum"/> with SVG-options.</typeparam>
    /// <typeparam name="TValue">Type of the value that is used when SVG-option is null.</typeparam>
    public struct SvgMixedEnum<TEnum, TValue> 
        where TEnum : struct, Enum
        where TValue : struct
    {
        /// <summary>
        /// Selected SVG-option. If <see langword="null"/>, then Value will be used instead.
        /// </summary>
        public TEnum? Option;
        /// <summary>
        /// Value that is used when Option is <see langword="null"/>.
        /// </summary>
        public TValue? Value;

        public SvgMixedEnum(TEnum? option = null, TValue? value = null)
        {
            Option = option;
            Value = value;
        }

        /// <summary>
        /// Get SVG-property value.
        /// </summary>
        /// <returns>
        /// 'Value', if 'Option' is <see langword="null"/>; 'Option' otherwise.
        /// </returns>
        public readonly override string? ToString()
        {
            if (Option != null) return Option.GetSvgOption();
            return Convert.ToString(Value, CultureInfo.InvariantCulture);
        }
    }
}
