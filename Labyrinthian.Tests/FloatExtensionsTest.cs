﻿using NUnit.Framework;

namespace Labyrinthian.Tests
{
    internal class FloatExtensionsTest
    {
        [Test]
        public void ToInvariantString()
        {
            float f = 1.61803f;
            Assert.That(f.ToInvariantString(), Is.EqualTo("1.61803"));
        }

        [Test]
        public void ApproximatelyEquals()
        {
            float a = 2.71828f, b = 2.71828f, c = 2.5f;

            Assert.Multiple(() =>
            {
                Assert.That(a.ApproximatelyEquals(b), Is.True);
                Assert.That(a.ApproximatelyEquals(c), Is.False);
            });
        }

        [Test]
        public void ApproximatelyEquals_CustomEpsilon()
        {
            float pi = 3.14159265359f, b = 3.14159292035f, c = 3f;

            Assert.Multiple(() =>
            {
                Assert.That(pi.ApproximatelyEquals(b, 1E-5f), Is.True);
                Assert.That(pi.ApproximatelyEquals(c, 1E-2f), Is.False);
            });
        }
    }
}
