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

        [TestMethod]
        public void Weapon_Initializes()
        {
            var type = new WeaponType("name", 'c', 10, 10, 10);
            try
            {
                var weapon = new Weapon(null, new Point(0, 0), type, null);
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
