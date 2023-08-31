namespace Labyrinthian.Svg
{
    /// <summary>
    /// Class that represents svg-fill.
    /// </summary>
    public abstract class SvgFill
    {
        public abstract override string ToString();

        public static readonly SvgFill None = new SvgFillNone();

        private sealed class SvgFillNone : SvgFill
        {
            public override string ToString() => "none";
        }
    }
}
