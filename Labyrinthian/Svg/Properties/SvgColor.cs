using System;
using System.Globalization;

namespace Labyrinthian.Svg
{
    public readonly struct SvgColor
    {
        public static readonly SvgColor White = new SvgColor(255, 255, 255);
        public static readonly SvgColor Black = new SvgColor(0, 0, 0);
        public static readonly SvgColor Red = new SvgColor(255, 0, 0);
        public static readonly SvgColor Green = new SvgColor(0, 255, 0);
        public static readonly SvgColor Blue = new SvgColor(0, 0, 255);

        public readonly byte R, G, B;

        private static byte TryParseToken(string token)
        {
            if (!byte.TryParse(token, NumberStyles.HexNumber,
                CultureInfo.InvariantCulture, out byte result))
            {
                throw new ArgumentException($"{token} is not a hex number");
            }

            return result;
        }

        /// <summary>
        /// Create a color from rgb values.
        /// </summary>
        /// <param name="red">Red(0-255).</param>
        /// <param name="green">Green(0-255).</param>
        /// <param name="blue">Blue(0-255)</param>
        public SvgColor(byte red, byte green, byte blue) : this()
        {
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Create a color from a hex code.
        /// </summary>
        /// <param name="hexCode">
        /// Hex code of the color in '#RRGGBB' format.
        /// '#RGB' is not supported and '#' is mandatory at the beginning.
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        public static SvgColor FromHexCode(string hexCode)
        {
            hexCode = hexCode.ToLower().Trim();
            if (hexCode.Length != 7 || hexCode[0] != '#')
            {
                throw new ArgumentException("Invalid color's hex code", nameof(hexCode));
            }

            byte R = TryParseToken(hexCode[1..3]);
            byte G = TryParseToken(hexCode[3..5]);
            byte B = TryParseToken(hexCode[5..7]);

            return new SvgColor(R, G, B);
        }

        /// <summary>
        /// Get hex code of the color.
        /// </summary>
        /// <returns>
        /// Color in '#RRGGBB' format. 
        /// </returns>
        public override string ToString()
        {
            return $"#{R:X2}{G:X2}{B:X2}";
        }
    }
}
