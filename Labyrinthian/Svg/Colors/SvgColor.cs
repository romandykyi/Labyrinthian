using System;
using System.Globalization;

namespace Labyrinthian.Svg
{
    public readonly struct SvgColor
    {
        public static readonly SvgColor White = new SvgColor(255, 255, 255, 255);
        public static readonly SvgColor Black = new SvgColor(0, 0, 0, 255);
        public static readonly SvgColor Red = new SvgColor(255, 0, 0, 255);
        public static readonly SvgColor Green = new SvgColor(0, 255, 0, 255);
        public static readonly SvgColor Blue = new SvgColor(0, 0, 255, 255);
        public static readonly SvgColor Transparent = new SvgColor(255, 255, 255, 0);
        public static readonly SvgColor Clear = new SvgColor(0, 0, 0, 0);


        public readonly byte R, G, B, A;

        public float Opacity => A / 255f;

        public bool IsOpaque => A == 255;

        private byte ParseHexToken(string token)
        {
            if (!byte.TryParse(token, NumberStyles.HexNumber,
                CultureInfo.InvariantCulture, out byte result))
            {
                throw new ArgumentException($"{token} != hex number", nameof(token));
            }

            return result;
        }

        /// <summary>
        /// Create a color from rgba values.
        /// </summary>
        /// <param name="red">Red(0-255).</param>
        /// <param name="green">Green(0-255).</param>
        /// <param name="blue">Blue(0-255)</param>
        /// <param name="alpha">Alpha(0-255). 255 by default.</param>
        public SvgColor(byte red, byte green, byte blue, byte alpha = 255) : this()
        {
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        /// <summary>
        /// Create a color from hexcode '#RRGGBB' or '#RRGGBBAA'.
        /// </summary>
        /// <param name="hexCode">
        /// Hexcode of the color in #RRGGBB or #RRGGBBAA format.
        /// #RGB' and '#RGBA' are not supported and '#' is mandatory at the beginning.
        /// </param>
        /// <exception cref="ArgumentException"></exception>
        public SvgColor(string hexCode) : this()
        {
            hexCode = hexCode.ToLower().Trim();
            if ((hexCode.Length != 7 && hexCode.Length != 9) || hexCode[0] != '#')
            {
                throw new ArgumentException("Invalid color's hex code", nameof(hexCode));
            }

            R = ParseHexToken(hexCode[1..3]);
            G = ParseHexToken(hexCode[3..5]);
            B = ParseHexToken(hexCode[5..7]);
            A = hexCode.Length == 9 ? ParseHexToken(hexCode[7..9]) : (byte)0xFF;
        }

        /// <summary>
        /// Get hexcode of the color.
        /// </summary>
        /// <returns>
        /// '#RRGGBB' format if alpha is 255, otherwise '#RRGGBBAA'. 
        /// </returns>
        public override string ToString()
        {
            return A == 255 ? $"#{R:X2}{G:X2}{B:X2}" : 
                $"#{R:X2}{G:X2}{B:X2}{A:X2}";
        }

        public static implicit operator SvgColor(string str) => new SvgColor(str);
    }
}