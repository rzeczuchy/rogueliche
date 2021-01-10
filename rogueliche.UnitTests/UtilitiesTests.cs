using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class UtilitiesTests
    {
        [TestMethod]
        public void Clamp_ReturnsClampedPositive()
        {
            Assert.AreEqual(Utilities.Clamp(1, 0, 2), 1);
            Assert.AreEqual(Utilities.Clamp(1, 5, 10), 5);
            Assert.AreEqual(Utilities.Clamp(30, 10, 20), 20);
        }

        [TestMethod]
        public void Clamp_ReturnsClampedNegative()
        {
            Assert.AreEqual(Utilities.Clamp(-1, -2, 0), -1);
            Assert.AreEqual(Utilities.Clamp(-20, -15, -10), -15);
            Assert.AreEqual(Utilities.Clamp(-30, -25, -20), -25);
        }
    }
}
