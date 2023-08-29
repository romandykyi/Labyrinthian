namespace Labyrinthian
{
    /// <summary>
    /// Class that represents svg-fill.
    /// </summary>
    public abstract class SvgFill
    {
        /// <summary>
        /// Value returned by this method will be used inside 'fill' or 'stroke' attribute.
        /// </summary>
        public abstract override string ToString();

        /// <summary>
        /// Definition that is written inside &lt;defs&gt;.
        /// <see langword="null" /> if definition is not needed for the fill.
        /// </summary>
        public virtual string? Definition => null;
        /// <summary>
        /// Opacity. 
        /// <see langword="null" /> if fill is opaque.
        /// </summary>
        public virtual string? Opacity => null;

        /// <summary>
        /// SVG 'none' fill.
        /// </summary>
        public static readonly SvgFill None = new SvgFillNone();

        private sealed class SvgFillNone : SvgFill
        {
            public override string ToString() => "none";
        }
    }
}
