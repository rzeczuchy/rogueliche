using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void Clamp_ReturnsClampedPositive()
        {
            Assert.AreEqual(Numbers.Clamp(1, 0, 2), 1);
            Assert.AreEqual(Numbers.Clamp(1, 5, 10), 5);
            Assert.AreEqual(Numbers.Clamp(30, 10, 20), 20);
        }

        [TestMethod]
        public void Clamp_ReturnsClampedNegative()
        {
            Assert.AreEqual(Numbers.Clamp(-1, -2, 0), -1);
            Assert.AreEqual(Numbers.Clamp(-20, -15, -10), -15);
            Assert.AreEqual(Numbers.Clamp(-30, -25, -20), -25);
        }
    }
}
