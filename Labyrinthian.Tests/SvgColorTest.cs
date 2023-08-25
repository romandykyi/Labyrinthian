using NUnit.Framework;
using System;

namespace Labyrinthian.Tests
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
            Assert.That(color.R == 0x02 && color.G == 0xAb && color.B == 0x4F && color.A == 0x88, Is.True);
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
