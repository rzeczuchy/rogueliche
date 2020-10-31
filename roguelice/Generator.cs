using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class Generator
    {
        const int SpecialMonsterChance = 20;
        const int SpecialWeaponChance = 20;
        private readonly List<MonsterSpecies> mnsSpecies;
        private readonly List<MonsterModifier> mnsModifiers;
        private readonly List<WeaponType> wpnTypes;
        private readonly List<WeaponModifier> wpnModifiers;

        public Generator()
        {
            MonsterSpecies slime = new MonsterSpecies("slime", 's', 6, 2, 5, 3);
            MonsterSpecies pest = new MonsterSpecies("pest", 'p', 10, 3, 5, 10);
            MonsterSpecies ghoul = new MonsterSpecies("ghoul", 'g', 22, 4, 3, 15);
            MonsterSpecies troll = new MonsterSpecies("troll", 't', 28, 4, 5, 20);
            MonsterSpecies harpy = new MonsterSpecies("harpy", 'h', 35, 5, 8, 25);
            MonsterSpecies minotaur = new MonsterSpecies("minotaur", 'm', 38, 6, 5, 40);
            MonsterSpecies cyclops = new MonsterSpecies("cyclops", 'm', 40, 7, 5, 65);
            MonsterSpecies werewolf = new MonsterSpecies("werewolf", 'w', 45, 8, 8, 75);
            MonsterSpecies drake = new MonsterSpecies("drake", 'd', 55, 10, 8, 100);
            mnsSpecies = new List<MonsterSpecies>() { slime, pest, ghoul, troll, harpy, minotaur, cyclops,
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
            mnsModifiers = new List<MonsterModifier>() { feeble, weak, tough, noxious, monstrous, ancient,
                undying, vicious, allseeing, murderous };

            WeaponType knife = new WeaponType("knife", 'K', 2, 1, 20);
            WeaponType bludgeon = new WeaponType("bludgeon", 'B', 4, 3, 25);
            WeaponType mace = new WeaponType("mace", 'M', 5, 6, 40);
            WeaponType axe = new WeaponType("axe", 'A', 6, 6, 30);
            WeaponType claymore = new WeaponType("claymore", 'C', 10, 8, 30);
            WeaponType trident = new WeaponType("trident", 'T', 8, 6, 25);
            WeaponType flail = new WeaponType("flail", 'F', 8, 6, 35);
            wpnTypes = new List<WeaponType>() { knife, bludgeon, mace, axe, claymore, trident, flail };

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
            wpnModifiers = new List<WeaponModifier>() { stinging, bloodthirsty, sturdy, unbreakable,
                quality, masterful, worn, tattered, crystal, spirit, light, heavy };
        }

        public Monster NewMonster(ILocation level, Point position, int floor)
        {
            MonsterModifier mod;
            mod = Numbers.RandomNumber(1, 100) <= SpecialMonsterChance ?
                mnsModifiers[Numbers.RandomNumber(0, mnsModifiers.Count - 1)] : null;

            return new Monster(level, position, mnsSpecies[LevelFactor(floor, mnsSpecies.Count())], mod);
        }

        public Weapon NewWeapon(ILocation level, Point position, int floor)
        {
            WeaponModifier mod;
            mod = Numbers.RandomNumber(1, 100) <= SpecialWeaponChance ?
                wpnModifiers[Numbers.RandomNumber(0, wpnModifiers.Count - 1)] : null;

            return new Weapon(level, position, wpnTypes[LevelFactor(floor, wpnTypes.Count())], mod);
        }

        private int LevelFactor(int floor, int listCount)
        {
            int min = 0;
            int max = floor / 3 + 1 < listCount - 1 ? floor / 3 + 1 : listCount - 1;

            if (floor / 3 - 5 < listCount - 1 && floor / 3 - 5 > 0)
                min = floor / 3 - 5;
            else if (floor / 3 - 5 >= listCount - 1)
                min = listCount - 6;

            return Numbers.RandomNumber(min, max);
        }
    }
}
