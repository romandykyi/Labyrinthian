
namespace Labyrinthian
{
    public sealed class SvgColorFill : SvgFill
    {
        public readonly SvgColor Color;

        public SvgColorFill(SvgColor color)
        {
            Color = color;
        }

        public override string ToString() => Color.ToString();

        public override string? Opacity => !Color.IsOpaque ? 
            Color.Opacity.ToInvariantString() : null;
    }
}