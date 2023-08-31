using Labyrinthian.Svg;
using NUnit.Framework;
using System.Globalization;

namespace Labyrinthian.Tests.Svg
{
    internal class SvgLengthTest
    {
        [Test]
        public void ToStringWorksCorrectly()
        {
            var previousCulture = CultureInfo.CurrentCulture;

            // In Polish culture ',' is used instead of '.' when displaying floating-point numbers
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pl-PL");

            SvgLength none = new(79.4f);
            SvgLength cm = new(18f, SvgLengthUnit.Centimeter);
            SvgLength mm = new(0.3f, SvgLengthUnit.Millimeter);
            SvgLength inch = new(3.3f, SvgLengthUnit.Inch);
            SvgLength em = new(0.6f, SvgLengthUnit.Em);
            SvgLength ex = new(0f, SvgLengthUnit.Ex);
            SvgLength pt = new(21.37f, SvgLengthUnit.Point);
            SvgLength pc = new(13.8f, SvgLengthUnit.Pica);
            SvgLength px = new(640f, SvgLengthUnit.Pixel);
            SvgLength perc = new(100f, SvgLengthUnit.Percentage);

            Assert.Multiple(() =>
            {
                Assert.That(none.ToString(), Is.EqualTo("79.4"));
                Assert.That(cm.ToString(), Is.EqualTo("18cm"));
                Assert.That(mm.ToString(), Is.EqualTo("0.3mm"));
                Assert.That(inch.ToString(), Is.EqualTo("3.3in"));
                Assert.That(em.ToString(), Is.EqualTo("0.6em"));
                Assert.That(ex.ToString(), Is.EqualTo("0ex"));
                Assert.That(pt.ToString(), Is.EqualTo("21.37pt"));
                Assert.That(pc.ToString(), Is.EqualTo("13.8pc"));
                Assert.That(px.ToString(), Is.EqualTo("640px"));
                Assert.That(perc.ToString(), Is.EqualTo("100%"));
            });
            CultureInfo.CurrentCulture = previousCulture;
        }
    }
}
