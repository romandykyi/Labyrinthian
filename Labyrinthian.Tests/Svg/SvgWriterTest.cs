using Labyrinthian.Svg;
using NUnit.Framework;
using System.IO;
using System.Xml.Linq;

namespace Labyrinthian.Tests.Svg
{
    internal class SvgWriterTest
    {
        [Test]
        public void SimplePicture()
        {
            MemoryStream ms = new();
            SvgWriter writer = new(ms);

            SvgRoot root = new()
            {
                ViewBox = new(0f, 0f, 600f, 600f)
            };
            SvgCircle wheel = new()
            {
                Id = "wheel",
                R = 50f,
                Stroke = new SvgColorFill(SvgColor.White),
                StrokeWidth = 15f,
                StrokeDasharray = new SvgLength[2] { new(5f, SvgLengthUnit.Pixel), new(12f, SvgLengthUnit.Point) },
                StrokeOpacity = 0.5f
            };
            SvgPath path = new()
            {
                Stroke = new SvgColorFill(SvgColor.Blue),
                StrokeWidth = 2f,
                D = "M 150,300 H 450"
            };
            SvgPolygon polygon = new()
            {
                Points = new SvgPoint[3] { new(50, 50), new(150,50), new(150,150) },
                Fill = new SvgColorFill(SvgColor.FromHexCode("#123456"))
            };
            SvgUse wheel1 = new()
            {
                UsedElement = wheel,
                X = 150f,
                Y = 300f,
                StrokeLinecap = SvgLinecap.Round
            };
            SvgUse wheel2 = new()
            {
                UsedElement = wheel,
                X = 450f,
                Y = 300f
            };

            writer.StartRoot(root);
            writer.WriteStringElement("desc", "Just testing");

            writer.StartElement(path);
            writer.EndElement();

            writer.StartElement(polygon);
            writer.EndElement();

            writer.StartElement(wheel1);
            writer.EndElement();

            writer.StartElement(wheel2);
            writer.EndElement();

            writer.EndRoot();

            writer.Close();

            ms.Position = 0;
            XDocument actualDocument = XDocument.Load(ms);
            XDocument expectedDocument = XDocument.Parse("<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" version=\"1.1\" viewBox=\"0 0 600 600\"><desc>Just testing</desc><path d=\"M 150,300 H 450\" stroke=\"#0000FF\" stroke-width=\"2\"/><polygon points=\"50,50 150,50 150,150\" fill=\"#123456\"/><use href=\"#wheel\" x=\"150\" y=\"300\" stroke-linecap=\"round\"/><use href=\"#wheel\" x=\"450\" y=\"300\"/><defs><circle r=\"50\" stroke=\"#FFFFFF\" stroke-width=\"15\" stroke-opacity=\"0.5\" stroke-dasharray=\"5px,12pt\" id=\"wheel\"/></defs></svg>");
            Assert.That(XNode.DeepEquals(actualDocument, expectedDocument), Is.True);
            
            ms.Close();
        }
    }
}
