﻿namespace Labyrinthian.Svg
{
    [SvgElement("use")]
    public sealed class SvgUse : SvgPresentationElement
    {
        public class ElementHref : INeedsDefinition
        {
            public SvgElement? Definition { get; set; }

            public override string ToString()
            {
                return $"#{Definition!.Id!}";
            }
        }

        public SvgElement? UsedElement { get; set; }

        [SvgProperty("href", Prefix = "xlink")]
        public ElementHref? Href
        {
            get
            {
                if (UsedElement == null) return null;
                if (UsedElement.Id == null) return null;
                return new ElementHref() { Definition = UsedElement };
            }
        }
        [SvgProperty("x")]
        public SvgLength? X { get; set; }
        [SvgProperty("y")]
        public SvgLength? Y { get; set; }
    }
}
