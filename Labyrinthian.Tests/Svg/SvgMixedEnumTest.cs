using System.Globalization;
using NUnit.Framework;
using Labyrinthian.Svg;

namespace Labyrinthian.Tests.Svg
{
    internal class SvgMixedEnumTest
    {
        private enum TestEnum
        {
            [SvgOption("test0")]
            Test0 = 0,
            [SvgOption("test1")]
            Test1 = 1,
            [SvgOption("test2")]
            Test2 = 2,
        }

        [Test]
        public void OptionDisplaysCorrectly()
        {
            SvgMixedEnum<TestEnum, float> mixedEnum = new(TestEnum.Test2);
            Assert.That(mixedEnum.ToString(), Is.EqualTo("test2"));
        }

        [Test]
        public void ValueDisplaysCorrectly()
        {
            SvgMixedEnum<TestEnum, float> mixedEnum = new(value: 13.3f);
            Assert.That(mixedEnum.ToString(), Is.EqualTo("13.3"));
        }
    }
}
