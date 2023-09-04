namespace Labyrinthian.Svg
{
    /// <summary>
    /// Interface for SVG-property that needs a definition inside &lt;defs&gt;.
    /// </summary>
    public interface INeedsDefinition
    {
        /// <summary>
        /// Definition of SVG-property to be written inside &lt;defs&gt;.
        /// Will be ignored if <see langword="null" />.
        /// </summary>
        SvgElement? Definition { get; }
    }
}
