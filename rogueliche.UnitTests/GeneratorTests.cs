using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void NewMonster_ReturnsMonster()
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
        public void NewWeapon_ReturnsWeapon()
        {
            var generator = new Generator();
            var level = new DungeonLevel(new Dungeon());
            var position = new Point(1, 1);
            var weapon = generator.NewRandomWeapon(level, position, 1);

            Assert.IsInstanceOfType(weapon, typeof(Weapon));
            Assert.AreEqual(weapon.Location, level);
            Assert.AreEqual(weapon.Position, position);
        }
    }
}
