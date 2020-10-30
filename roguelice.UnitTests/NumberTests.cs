using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace roguelice.UnitTests
{
    [TestClass]
    public class NumberTests
    {
        [TestMethod]
        public void Clamp_ReturnsClampedNumber()
        {
            Assert.AreEqual(Numbers.Clamp(1, 0, 2), 1);
            Assert.AreEqual(Numbers.Clamp(1, -3, -1), -1);
            Assert.AreEqual(Numbers.Clamp(30, 10, 20), 20);
        }
    }
}
