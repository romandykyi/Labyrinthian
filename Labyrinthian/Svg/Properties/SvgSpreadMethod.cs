namespace Labyrinthian.Svg
{
    public readonly struct SvgSpreadMethod
    {
        private enum SpreadMethod
        {
            Pad, Reflect, Repeat
        }

        private readonly SpreadMethod _value;

        private SvgSpreadMethod(SpreadMethod value)
        {
            _value = value;
        }

        public static SvgSpreadMethod Pad => new SvgSpreadMethod(SpreadMethod.Pad);
        public static SvgSpreadMethod Reflect => new SvgSpreadMethod(SpreadMethod.Reflect);
        public static SvgSpreadMethod Repeat => new SvgSpreadMethod(SpreadMethod.Repeat);

        public override string? ToString()
        {
            return _value switch
            {
                SpreadMethod.Pad => "pad",
                SpreadMethod.Repeat => "repeat",
                SpreadMethod.Reflect => "reflect",
                _ => null
            };
        }
    }
}
