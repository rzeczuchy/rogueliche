using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class Generator
    {
        const int SpecialMonsterChance = 100;
        const int SpecialWeaponChance = 100;
        private readonly List<MonsterType> monsterTypes;
        private readonly List<MonsterModifier> monsterModifiers;
        private readonly List<WeaponType> weaponTypes;
        private readonly List<WeaponModifier> weaponModifiers;

        public Generator()
        {
            MonsterType slime = new MonsterType("slime", 's', 6, 2, 5, 3);
            MonsterType pest = new MonsterType("pest", 'p', 10, 3, 5, 10);
            MonsterType ghoul = new MonsterType("ghoul", 'g', 22, 4, 3, 15);
            MonsterType troll = new MonsterType("troll", 't', 28, 4, 5, 20);
            MonsterType harpy = new MonsterType("harpy", 'h', 35, 5, 8, 25);
            MonsterType minotaur = new MonsterType("minotaur", 'm', 38, 6, 5, 40);
            MonsterType cyclops = new MonsterType("cyclops", 'm', 40, 7, 5, 65);
            MonsterType werewolf = new MonsterType("werewolf", 'w', 45, 8, 8, 75);
            MonsterType drake = new MonsterType("drake", 'd', 55, 10, 8, 100);
            monsterTypes = new List<MonsterType>() { slime, pest, ghoul, troll, harpy, minotaur, cyclops,
                werewolf, drake };

            MonsterModifier feeble = new MonsterModifier("feeble", 0.5, 0.7, 1, 0.6);
            MonsterModifier weak = new MonsterModifier("weak", 0.7, 0.7, 1, 0.9);
            MonsterModifier tough = new MonsterModifier("tough", 1.3, 1, 1, 1.2);
            MonsterModifier noxious = new MonsterModifier("noxious", 1.1, 1.2, 1, 1.3);
            MonsterModifier monstrous = new MonsterModifier("monstrous", 1.3, 1.2, 1, 1.5);
            MonsterModifier ancient = new MonsterModifier("ancient", 1.5, 1.1, 1, 1.7);
            MonsterModifier undying = new MonsterModifier("undying", 2.5, 0.5, 1, 1.8);
            MonsterModifier vicious = new MonsterModifier("vicious", 0.7, 1.8, 1.3, 1.8);
            MonsterModifier allseeing = new MonsterModifier("allseeing", 1, 1, 3, 1.4);
            MonsterModifier murderous = new MonsterModifier("murderous", 1, 2.2, 1.5, 2.1);
            monsterModifiers = new List<MonsterModifier>() { feeble, weak, tough, noxious, monstrous, ancient,
                undying, vicious, allseeing, murderous };

            WeaponType knife = new WeaponType("knife", 'K', 2, 1, 20);
            WeaponType bludgeon = new WeaponType("bludgeon", 'B', 4, 3, 25);
            WeaponType mace = new WeaponType("mace", 'M', 5, 6, 40);
            WeaponType axe = new WeaponType("axe", 'A', 6, 6, 30);
            WeaponType claymore = new WeaponType("claymore", 'C', 10, 8, 30);
            WeaponType trident = new WeaponType("trident", 'T', 8, 6, 25);
            WeaponType flail = new WeaponType("flail", 'F', 8, 6, 35);
            weaponTypes = new List<WeaponType>() { knife, bludgeon, mace, axe, claymore, trident, flail };

            WeaponModifier stinging = new WeaponModifier("stinging", 1.3, 1, 1);
            WeaponModifier bloodthirsty = new WeaponModifier("bloodthirsty", 1.6, 1, 1);
            WeaponModifier sturdy = new WeaponModifier("sturdy", 1, 1.2, 1.7);
            WeaponModifier unbreakable = new WeaponModifier("unbreakable", 1, 1.5, 2.5);
            WeaponModifier quality = new WeaponModifier("quality", 1.3, 1, 1.3);
            WeaponModifier masterful = new WeaponModifier("masterful", 1.6, 1, 1.6);
            WeaponModifier worn = new WeaponModifier("worn", 0.8, 1, 0.9);
            WeaponModifier tattered = new WeaponModifier("tattered", 0.6, 1, 0.7);
            WeaponModifier crystal = new WeaponModifier("crystal", 1.7, 0.5, 0.4);
            WeaponModifier spirit = new WeaponModifier("spirit", 2.5, 0.2, 0.2);
            WeaponModifier light = new WeaponModifier("light", 0.8, 0.8, 1);
            WeaponModifier heavy = new WeaponModifier("heavy", 1.5, 1.5, 1);
            weaponModifiers = new List<WeaponModifier>() { stinging, bloodthirsty, sturdy, unbreakable,
                quality, masterful, worn, tattered, crystal, spirit, light, heavy };
        }

        public Monster NewMonster(ILocation level, Point position, int floor)
        {
            var modifier = Utilities.PassPercentileRoll(SpecialMonsterChance) ?
                Utilities.GetRandomFromList(monsterModifiers): null;

            return new Monster(level, position, monsterTypes[LevelFactor(floor, monsterTypes.Count())], modifier);
        }

        public Weapon NewWeapon(ILocation level, Point position, int floor)
        {
            var modifier = Utilities.PassPercentileRoll(SpecialWeaponChance) ?
                Utilities.GetRandomFromList(weaponModifiers) : null;

            return new Weapon(level, position, weaponTypes[LevelFactor(floor, weaponTypes.Count())], modifier);
        }

        private int LevelFactor(int floor, int listCount)
        {
            int min = 0;
            int max = floor / 3 + 1 < listCount - 1 ? floor / 3 + 1 : listCount - 1;

            if (floor / 3 - 5 < listCount - 1 && floor / 3 - 5 > 0)
                min = floor / 3 - 5;
            else if (floor / 3 - 5 >= listCount - 1)
                min = listCount - 6;

            return Utilities.RandomNumber(min, max);
        }
    }
}
