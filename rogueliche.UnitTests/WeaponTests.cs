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

        [TestMethod]
        public void Damage_ReturnsCorrectDamage()
        {
            int damage = 10;
            var type = new WeaponType("name", 'c', damage, 10, 10);
            var weapon = new Weapon(null, new Point(0, 0), type, null);

            Assert.AreEqual(weapon.Damage, 10);
        }

        [TestMethod]
        public void DecreaseDurability_WorksWhenPlayerNull()
        {
            var type = new WeaponType("name", 'c', 10, 10, 10);
            var weapon = new Weapon(null, new Point(0, 0), type, null);

            try
            {
                weapon.DecreaseDurability(1, null);
                weapon.DecreaseDurability(weapon.Durability, null);
            }
            catch
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void DecreaseDurability_DecreasesWhenNotBroken()
        {
            int durability = 10;
            var type = new WeaponType("name", 'c', 10, 10, durability);
            var weapon = new Weapon(null, new Point(0, 0), type, null);

            weapon.DecreaseDurability(1, null);
            Assert.AreEqual(weapon.Durability, durability - 1);

            weapon.DecreaseDurability(weapon.Durability, null);
            Assert.AreEqual(weapon.Durability, 0);
        }

        [TestMethod]
        public void Place_ThrowsExceptionWhenTargetLocationNull()
        {
            var type = new WeaponType("name", 'c', 10, 10, 10);
            var weapon = new Weapon(null, new Point(0, 0), type, null);

            Assert.ThrowsException<ArgumentNullException>(() => { weapon.Place(null, new Point(0, 0)); });
        }
    }
}
