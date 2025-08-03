using NUnit.Framework;

namespace Labyrinthian.Tests.DataStructures
{
    internal class DisjointSetTest
    {
        [Test]
        public void Add()
        {
            int[] arr = [0, 1, 2, 3];
            DisjointSet<int> set = [];
            foreach (int i in arr)
            {
                Assert.That(set.Add(i), Is.True);
            }

            using (Assert.EnterMultipleScope())
            {
                Assert.That(set.Add(arr[0]), Is.False);
                Assert.That(set, Is.EquivalentTo(arr));
            }
        }

        [Test]
        public void IEnumerableConstructor()
        {
            int[] arr = [0, 1, 2, 3];
            DisjointSet<int> set = [0, 1, 2, 3];

            Assert.That(set, Is.EquivalentTo(arr));
        }

        [Test]
        public void Contains()
        {
            DisjointSet<char> set = ['a', 'b', 'c', 'd'];

            using (Assert.EnterMultipleScope())
            {
                Assert.That(set.Contains('a'), Is.True);
                Assert.That(set.Contains('c'), Is.True);
                Assert.That(set.Contains('d'), Is.True);
                Assert.That(set.Contains('e'), Is.False);
                Assert.That(set.Contains('0'), Is.False);
            }
        }

        [Test]
        public void Union()
        {
            DisjointSet<int> set = [1, 2, 3];

            using (Assert.EnterMultipleScope())
            {
                Assert.That(set.Union(1, 2), Is.True);
                Assert.That(set.Union(2, 1), Is.False);
                Assert.That(set.Union(1, 3), Is.True);
                Assert.That(set.Union(2, 3), Is.False);
                Assert.That(set.Union(1, 1), Is.False);
            }
        }

        [Test]
        public void Find()
        {
            DisjointSet<int> set = [1, 2, 3, 4, 5, 6, 7, 8, 9];

            // 1-2-3
            set.Union(1, 2);
            set.Union(1, 3);
            // 4-5-6
            set.Union(4, 5);
            set.Union(5, 6);
            // 7-8
            set.Union(7, 8);

            using (Assert.EnterMultipleScope())
            {
                // 1-2-3
                Assert.That(set.Find(1), Is.EqualTo(set.Find(2)));
                Assert.That(set.Find(2), Is.EqualTo(set.Find(3)));
                // 4-5-6
                Assert.That(set.Find(4), Is.EqualTo(set.Find(5)));
                Assert.That(set.Find(5), Is.EqualTo(set.Find(6)));
                // 7-8
                Assert.That(set.Find(7), Is.EqualTo(set.Find(8)));

                // 7-8 != 9
                Assert.That(set.Find(7), Is.Not.EqualTo(set.Find(9)));
                // 7-8 != 1-2-3
                Assert.That(set.Find(8), Is.Not.EqualTo(set.Find(3)));
                // 1-2-3 != 4-5-6
                Assert.That(set.Find(2), Is.Not.EqualTo(set.Find(6)));
                // 4-5-6 != 9
                Assert.That(set.Find(6), Is.Not.EqualTo(set.Find(9)));
            }
        }
    }
}
