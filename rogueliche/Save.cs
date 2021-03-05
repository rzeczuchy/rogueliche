using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class Save
    {
        public Save()
        {
        }

        public Save(Player player)
        {
            PlayerKillCount = player.KillCount;
            PlayerBrokenWeapons = player.BrokenWeapons;
            PlayerMaxHealth = player.MaxHealth;
            PlayerHealth = player.Health;
            PlayerLvl = player.Lvl;
            PlayerExp = player.Exp;
            PlayerExpToNextLvl = player.ExpToNextLvl;
            PlayerMaxExertion = player.MaxExertion;
            PlayerExertion = player.Exertion;
            PlayerWeaponType = player.CurrentWeapon.Type.Name;
            if (player.CurrentWeapon.Modifier != null)
            {
                PlayerWeaponModifier = player.CurrentWeapon.Modifier.NamePrefix;
            }
            else
            {
                PlayerWeaponModifier = null;
            }
            PlayerWeaponDurability = player.CurrentWeapon.Durability;
            LevelIndex = player.Location is DungeonLevel dungeonLevel ? (dungeonLevel.LevelIndex - 1) : 0;
        }

        public int PlayerKillCount { get; set; }
        public int PlayerBrokenWeapons { get; set; }
        public int PlayerMaxHealth { get; set; }
        public int PlayerHealth { get; set; }
        public int PlayerLvl { get; set; }
        public int PlayerExp { get; set; }
        public int PlayerExpToNextLvl { get; set; }
        public int PlayerExertion { get; set; }
        public int PlayerMaxExertion { get; set; }
        public string PlayerWeaponType { get; set; }
        public string PlayerWeaponModifier { get; set; }
        public int PlayerWeaponDurability { get; set; }
        public int LevelIndex { get; set; }
    }
}
