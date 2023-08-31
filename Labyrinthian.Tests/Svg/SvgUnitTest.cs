using Labyrinthian.Svg;
using NUnit.Framework;
using System.Globalization;

namespace Labyrinthian.Tests.Svg
{
    internal class SvgUnitTest
    {
        [Test]
        public void ActualValueDisplaysCorrectly()
        {
            SvgUnit unit = 12;
            Assert.That(unit.ToString(), Is.EqualTo("12"));
        }

        [Test]
        public void PercentageDisplaysCorrectly_()
        {
            SvgUnit percentageUnit = SvgUnit.FromPercentage(75.5f);
            Assert.That(percentageUnit.ToString(), Is.EqualTo("75.5%"));
        }

        [Test]
        public void PolishTest()
        {
            var previousCulture = CultureInfo.CurrentCulture;

            // In Polish culture ',' is used instead of '.' when displaying floating-point numbers
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pl-PL");

            SvgUnit numericalUnit = 3.14f;
            SvgUnit percentageUnit = SvgUnit.FromPercentage(12.4f);
            Assert.Multiple(() =>
            {
                Assert.That(numericalUnit.ToString(), Is.EqualTo("3.14"));
                Assert.That(percentageUnit.ToString(), Is.EqualTo("12.4%"));
            });
            CultureInfo.CurrentCulture = previousCulture;
        }

        [Test]
        public void ValueSavesActualValue()
        {
            SvgUnit percentageUnit = SvgUnit.FromPercentage(50f);
            Assert.That(percentageUnit.Value, Is.EqualTo(0.5f));
        }

        [Test]
        public void EqualsWorks()
        {
            SvgUnit numericalUnit1 = 1f;
            SvgUnit numericalUnit2 = 1f;
            SvgUnit percentageUnit1 = SvgUnit.FromPercentage(100f);
            SvgUnit percentageUnit2 = SvgUnit.FromPercentage(50f);

            Assert.Multiple(() =>
            {
                Assert.That(numericalUnit1, Is.EqualTo(numericalUnit2));
                Assert.That(numericalUnit1, Is.EqualTo(percentageUnit1));
                Assert.That(percentageUnit1, Is.Not.EqualTo(percentageUnit2));
            });
        }
    }
}
