using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class WeaponTests
    {
        [TestMethod]
        public void Constructor_ThrowsExceptionWhenTypeNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => { new Weapon(null, new Point(0, 0), null, null); });
        }
    }
}
