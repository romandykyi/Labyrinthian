namespace Labyrinthian.Svg
{
    public abstract class SvgShape : SvgGroup
    {
        [SvgProperty("pathLength")]
        public float? PathLength { get; set; }
    }
}
