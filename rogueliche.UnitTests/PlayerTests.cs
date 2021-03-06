using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Load_LoadsPlayerStateFromSave()
        {
            var killCount = 102;
            var brokenWeapons = 231;
            var maxHealth = 321;
            var health = 43;
            var lvl = 12;
            var exp = 5;
            var expToNextLvl = 123;
            var maxExertion = 100;
            var exertion = 50;
            var weaponType = "bludgeon";
            var weaponModifier = "stinging";
            var weaponDurability = 13;

            var save = new Save()
            {
                PlayerKillCount = killCount,
                PlayerBrokenWeapons = brokenWeapons,
                PlayerMaxHealth = maxHealth,
                PlayerHealth = health,
                PlayerLvl = lvl,
                PlayerExp = exp,
                PlayerExpToNextLvl = expToNextLvl,
                PlayerMaxExertion = maxExertion,
                PlayerExertion = exertion,
                PlayerWeaponType = weaponType,
                PlayerWeaponModifier = weaponModifier,
                PlayerWeaponDurability = weaponDurability,
            };

            var player = new Player(new EmptyLevel("name", 10, 10), new Point(1, 1));
            player.Load(save);

            Assert.AreEqual(killCount, player.KillCount);
            Assert.AreEqual(brokenWeapons, player.BrokenWeapons);
            Assert.AreEqual(maxHealth, player.MaxHealth);
            Assert.AreEqual(health, player.Health);
            Assert.AreEqual(lvl, player.Lvl);
            Assert.AreEqual(exp, player.Exp);
            Assert.AreEqual(expToNextLvl, player.ExpToNextLvl);
            Assert.AreEqual(maxExertion, player.MaxExertion);
            Assert.AreEqual(exertion, player.Exertion);
            Assert.AreEqual(weaponType, player.CurrentWeapon.Type.Name);
            Assert.AreEqual(weaponModifier, player.CurrentWeapon.Modifier.NamePrefix);
            Assert.AreEqual(weaponDurability, player.CurrentWeapon.Durability);
        }
    }
}
