using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void NewRandomMonster_ReturnsMonster()
        {
            var generator = new Generator();
            var level = new DungeonLevel(new Dungeon());
            var position = new Point(1, 1);
            var monster = generator.NewRandomMonster(level, position, 1);

            Assert.IsInstanceOfType(monster, typeof(Monster));
            Assert.AreEqual(monster.Location, level);
            Assert.AreEqual(monster.Position, position);
        }

        [TestMethod]
        public void NewRandomWeapon_ReturnsWeapon()
        {
            var generator = new Generator();
            var level = new DungeonLevel(new Dungeon());
            var position = new Point(1, 1);
            var weapon = generator.NewRandomWeapon(level, position, 1);

            Assert.IsInstanceOfType(weapon, typeof(Weapon));
            Assert.AreEqual(weapon.Location, level);
            Assert.AreEqual(weapon.Position, position);
        }

        [TestMethod]
        public void GetWeaponTypeByName_ReturnsCorrectWeaponType()
        {
            var generator = new Generator();
            var type = generator.Mace;
            Assert.AreEqual(type, generator.GetWeaponTypeByName(type.Name));
        }

        [TestMethod]
        public void WeaponTypeExists_ChecksForTypeInList()
        {
            var generator = new Generator();
            var result = generator.WeaponTypeExists("jabberwocky");
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GetWeaponModifierByName_ReturnsCorrectWeaponModifier()
        {
            var generator = new Generator();
            var modifier = generator.Stinging;
            Assert.AreEqual(modifier, generator.GetWeaponModifierByName(modifier.NamePrefix));
        }

        [TestMethod]
        public void WeaponModifierExists_ChecksForModifierInList()
        {
            var generator = new Generator();
            var result = generator.WeaponModifierExists("jabberwocky");
            Assert.AreEqual(false, result);
        }
    }
}
