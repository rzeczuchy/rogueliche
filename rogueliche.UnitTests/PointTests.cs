using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void Distance_MeasuresDistance()
        {
            Assert.AreEqual(Point.Distance(new Point(0, 0), new Point(0, 0)), 0);
            Assert.AreEqual(Point.Distance(new Point(0, 0), new Point(0, 3)), 3);
            Assert.AreEqual(Point.Distance(new Point(1, 0), new Point(-1, 0)), 2);
            Assert.AreEqual(Point.Distance(new Point(124, 0), new Point(-6, 0)), 130);
        }

        [TestMethod]
        public void Distance_ThrowsExceptionIfPointNull()
        {
            Assert.ThrowsException<NullReferenceException>(() => Point.Distance(null, null));
            Assert.ThrowsException<NullReferenceException>(() => Point.Distance(new Point(0, 0), null));
            Assert.ThrowsException<NullReferenceException>(() => Point.Distance(null, new Point(0, 0)));
        }
    }
}
