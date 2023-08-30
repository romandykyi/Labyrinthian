using NUnit.Framework;
using System;
using Labyrinthian.Svg;

namespace Labyrinthian.Tests.Svg
{
    internal class SvgColorTest
    {
        [Test]
        public void ParsingColorWithoutAlpha()
        {
            SvgColor color = new(" #FF0050  ");
            Assert.Multiple(() =>
            {
                Assert.That(color.R, Is.EqualTo(0xFF));
                Assert.That(color.G, Is.EqualTo(0x00));
                Assert.That(color.B, Is.EqualTo(0x50));
                Assert.That(color.A, Is.EqualTo(0xFF));
            });
        }

        [Test]
        public void ParsingColorWithAlpha()
        {
            SvgColor color = new("#02Ab4F88");
            Assert.Multiple(() =>
            {
                Assert.That(color.R, Is.EqualTo(0x02));
                Assert.That(color.G, Is.EqualTo(0xAB));
                Assert.That(color.B, Is.EqualTo(0x4F));
                Assert.That(color.A, Is.EqualTo(0x88));
            });
        }

        [Test]
        public void ParsingInvalidLengthColor()
        {
            Assert.Throws<ArgumentException>(() => new SvgColor("#04980"));
        }

        [Test]
        public void ParsingInvalidColor()
        {
            Assert.Throws<ArgumentException>(() => new SvgColor("ArtemMF"));
        }

        [Test]
        public void ParsingInvalidColor2()
        {
            Assert.Throws<ArgumentException>(() => new SvgColor("#ptnpnh"));
        }
    }
}
