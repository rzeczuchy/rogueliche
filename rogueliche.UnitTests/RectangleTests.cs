using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class RectangleTests
    {
        [TestMethod]
        public void Rectangle_ReturnsCorrectDimensions()
        {
            int width = 24;
            int height = 10;
            var pos = new Point(-12, 5);
            var center = new Point(pos.X + width / 2, pos.Y + height / 2);
            var rect = new Rectangle(pos, new Point(width, height));

            Assert.AreEqual(rect.Width, width);
            Assert.AreEqual(rect.Height, height);
            Assert.AreEqual(rect.Left, pos.X);
            Assert.AreEqual(rect.Right, pos.X + width);
            Assert.AreEqual(rect.Top, pos.Y);
            Assert.AreEqual(rect.Bottom, pos.Y + height);
            Assert.AreEqual(rect.Center.X, center.X);
            Assert.AreEqual(rect.Center.Y, center.Y);
        }
    }
}
