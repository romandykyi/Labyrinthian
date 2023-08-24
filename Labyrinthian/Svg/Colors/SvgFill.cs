
namespace Labyrinthian
{
    public abstract class SvgFill
    {
        public abstract override string ToString();

        public virtual string? Definition => null;
        public virtual string? Opacity => null;

        public static readonly SvgFill None = new SvgFillNone();

        private sealed class SvgFillNone : SvgFill
        {
            public override string ToString() => "none";
        }
    }
}