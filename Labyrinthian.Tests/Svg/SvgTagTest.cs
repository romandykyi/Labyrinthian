using NUnit.Framework;
using Labyrinthian.Svg;

namespace Labyrinthian.Tests.Svg
{
    internal class SvgTagTest
    {
        [Test]
        public void Empty()
        {
            SvgTag tag = new("path");
            Assert.That(tag.ToString(), Is.EqualTo("<path />"));
        }

        [Test]
        public void StringAttribute()
        {
            SvgTag tag = new("path");
            tag.AddAttribute("stroke", "black");
            Assert.That(tag.ToString(), Is.EqualTo("<path stroke=\"black\" />"));
        }

        [Test]
        public void FloatAttribute()
        {
            SvgTag tag = new("path");
            tag.AddAttribute("stroke-width", 2.5f);
            Assert.That(tag.ToString(), Is.EqualTo("<path stroke-width=\"2.5\" />"));
        }

        [Test]
        public void MultipleAttributes()
        {
            SvgTag tag = new("path");
            tag.AddAttribute("stroke", "orange");
            tag.AddAttribute("stroke-width", 2.4f);
            tag.AddAttribute("fill", "white");
            Assert.That(tag.ToString(), Is.EqualTo("<path stroke=\"orange\" stroke-width=\"2.4\" fill=\"white\" />"));
        }

        [Test]
        public void NestedTags_NoAttributes()
        {
            SvgTag parent = new("g");
            parent.Children.Add(new("path"));
            parent.Children.Add(new("circle"));
            parent.Children.Add(new("rect"));

            Assert.That(parent.ToString(), Is.EqualTo("<g><path /><circle /><rect /></g>"));
        }

        [Test]
        public void NestedTags_AttributesOnParent()
        {
            SvgTag parent = new("g");
            parent.AddAttribute("id", "g100");
            parent.Children.Add(new("polygon"));
            parent.Children.Add(new("polyline"));

            Assert.That(parent.ToString(), Is.EqualTo("<g id=\"g100\"><polygon /><polyline /></g>"));
        }

        [Test]
        public void NestedTags_WithAttributes()
        {
            SvgTag parent = new("g");
            parent.AddAttribute("id", "g100");
            SvgTag child1 = new("rect");
            child1.AddAttribute("fill", "red");
            SvgTag child2 = new("polyline");
            parent.Children.Add(child1);
            parent.Children.Add(child2);

            Assert.That(parent.ToString(), Is.EqualTo("<g id=\"g100\"><rect fill=\"red\" /><polyline /></g>"));
        }

        [Test]
        public void MultipleNestedTags()
        {
            SvgTag parent1 = new("g");
            parent1.AddAttribute("id", "main");
            SvgTag parent2 = new("g");
            SvgTag child1 = new("rect");
            child1.AddAttribute("fill", "black");
            SvgTag child2 = new("polyline");

            parent1.Children.Add(child1);
            parent1.Children.Add(parent2);
            parent2.Children.Add(child2);

            Assert.That(parent1.ToString(), Is.EqualTo("<g id=\"main\"><rect fill=\"black\" /><g><polyline /></g></g>"));
        }
    }
}
