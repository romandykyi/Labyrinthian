using Labyrinthian.Svg;
using NUnit.Framework;

namespace Labyrinthian.Tests.Svg
{
    internal class SvgPreserveAspectRatioTest
    {
        [Test]
        public void None()
        {
            SvgPreserveAspectRatio preserve = new(SvgAspectRatio.None, SvgPreserveMode.Meet);
            Assert.That(preserve.ToString(), Is.EqualTo("none"));
        }

        [Test]
        public void WithoutPreserveMode()
        {
            SvgPreserveAspectRatio preserve = new(SvgAspectRatio.XMidYMin);
            Assert.That(preserve.ToString(), Is.EqualTo("xMidYMin"));
        }

        [Test]
        public void WithPreserveMode()
        {
            SvgPreserveAspectRatio preserve = new(SvgAspectRatio.XMinYMax, SvgPreserveMode.Slice);
            Assert.That(preserve.ToString(), Is.EqualTo("xMinYMax slice"));
        }
    }
}
