using NUnit.Framework;
using System;
using Labyrinthian.Svg;

namespace Labyrinthian.Tests.Svg
{
    internal class SvgColorTest
    {
        [Test]
        public void ParsingColor()
        {
            SvgColor color = SvgColor.FromHexCode("#Ff0050");
            Assert.Multiple(() =>
            {
                Assert.That(color.R, Is.EqualTo(0xFF));
                Assert.That(color.G, Is.EqualTo(0x00));
                Assert.That(color.B, Is.EqualTo(0x50));
            });
        }

        [Test]
        public void ParsingInvalidLengthColor()
        {
            Assert.Throws<ArgumentException>(() => SvgColor.FromHexCode("#04980"));
        }

        [Test]
        public void ParsingInvalidColor()
        {
            Assert.Throws<ArgumentException>(() => SvgColor.FromHexCode("ArtemMF"));
        }

        [Test]
        public void ParsingInvalidColor2()
        {
            Assert.Throws<ArgumentException>(() => SvgColor.FromHexCode("#ptnpnh"));
        }

        [Test]
        public void ToStringReturnsRRGGBB()
        {
            SvgColor color = new(0x42, 0xAA, 0x90);
            Assert.That(color.ToString().ToUpper(), Is.EqualTo("#42AA90"));
        }
    }
}
