using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class Generator
    {
        const int SpecialMonsterChance = 25;
        const int SpecialWeaponChance = 25;

        public Generator()
        {
            Slime = new MonsterType("slime", 's', 6, 2, 5, 3);
            Pest = new MonsterType("pest", 'p', 10, 3, 5, 10);
            Ghoul = new MonsterType("ghoul", 'g', 22, 4, 3, 15);
            Troll = new MonsterType("troll", 't', 28, 4, 5, 20);
            Harpy = new MonsterType("harpy", 'h', 35, 5, 8, 25);
            Minotaur = new MonsterType("minotaur", 'm', 38, 6, 5, 40);
            Cyclops = new MonsterType("cyclops", 'm', 40, 7, 5, 65);
            Werewolf = new MonsterType("werewolf", 'w', 45, 8, 8, 75);
            Drake = new MonsterType("drake", 'd', 55, 10, 8, 100);
            MonsterTypes = new List<MonsterType>() { Slime, Pest, Ghoul, Troll, Harpy, Minotaur, Cyclops,
                Werewolf, Drake };

            Feeble = new MonsterModifier("feeble", 0.5, 0.7, 1, 0.6);
            Weak = new MonsterModifier("weak", 0.7, 0.7, 1, 0.9);
            Tough = new MonsterModifier("tough", 1.3, 1, 1, 1.2);
            Noxious = new MonsterModifier("noxious", 1.1, 1.2, 1, 1.3);
            Monstrous = new MonsterModifier("monstrous", 1.3, 1.2, 1, 1.5);
            Ancient = new MonsterModifier("ancient", 1.5, 1.1, 1, 1.7);
            Undying = new MonsterModifier("undying", 2.5, 0.5, 1, 1.8);
            Vicious = new MonsterModifier("vicious", 0.7, 1.8, 1.3, 1.8);
            Allseeing = new MonsterModifier("allseeing", 1, 1, 3, 1.4);
            Murderous = new MonsterModifier("murderous", 1, 2.2, 1.5, 2.1);
            MonsterModifiers = new List<MonsterModifier>() { Feeble, Weak, Tough, Noxious, Monstrous, Ancient,
                Undying, Vicious, Allseeing, Murderous };

            Knife = new WeaponType("knife", 'K', 2, 1, 20);
            Bludgeon = new WeaponType("bludgeon", 'B', 4, 3, 25);
            Mace = new WeaponType("mace", 'M', 5, 6, 40);
            Axe = new WeaponType("axe", 'A', 6, 6, 30);
            Claymore = new WeaponType("claymore", 'C', 10, 8, 30);
            Trident = new WeaponType("trident", 'T', 8, 6, 25);
            Flail = new WeaponType("flail", 'F', 8, 6, 35);
            WeaponTypes = new List<WeaponType>() { Knife, Bludgeon, Mace, Axe, Claymore, Trident, Flail };

            Stinging = new WeaponModifier("stinging", 1.3, 1, 1);
            Bloodthirsty = new WeaponModifier("bloodthirsty", 1.6, 1, 1);
            Sturdy = new WeaponModifier("sturdy", 1, 1.2, 1.7);
            Unbreakable = new WeaponModifier("unbreakable", 1, 1.5, 2.5);
            Quality = new WeaponModifier("quality", 1.3, 1, 1.3);
            Masterful = new WeaponModifier("masterful", 1.6, 1, 1.6);
            Worn = new WeaponModifier("worn", 0.8, 1, 0.9);
            Tattered = new WeaponModifier("tattered", 0.6, 1, 0.7);
            Crystal = new WeaponModifier("crystal", 1.7, 0.5, 0.4);
            Spirit = new WeaponModifier("spirit", 2.5, 0.2, 0.2);
            Light = new WeaponModifier("light", 0.8, 0.8, 1);
            Heavy = new WeaponModifier("heavy", 1.5, 1.5, 1);
            WeaponModifiers = new List<WeaponModifier>() { Stinging, Bloodthirsty, Sturdy, Unbreakable,
                Quality, Masterful, Worn, Tattered, Crystal, Spirit, Light, Heavy };
        }

        public List<MonsterType> MonsterTypes { get; }
        public List<MonsterModifier> MonsterModifiers { get; }
        public List<WeaponType> WeaponTypes { get; }
        public List<WeaponModifier> WeaponModifiers { get; }

        public MonsterType Slime { get; private set; }
        public MonsterType Pest { get; private set; }
        public MonsterType Ghoul { get; private set; }
        public MonsterType Troll { get; private set; }
        public MonsterType Harpy { get; private set; }
        public MonsterType Minotaur { get; private set; }
        public MonsterType Cyclops { get; private set; }
        public MonsterType Werewolf { get; private set; }
        public MonsterType Drake { get; private set; }

        public MonsterModifier Feeble { get; private set; }
        public MonsterModifier Weak { get; private set; }
        public MonsterModifier Tough { get; private set; }
        public MonsterModifier Noxious { get; private set; }
        public MonsterModifier Monstrous { get; private set; }
        public MonsterModifier Ancient { get; private set; }
        public MonsterModifier Undying { get; private set; }
        public MonsterModifier Vicious { get; private set; }
        public MonsterModifier Allseeing { get; private set; }
        public MonsterModifier Murderous { get; private set; }

        public WeaponType Knife { get; private set; }
        public WeaponType Bludgeon { get; private set; }
        public WeaponType Mace { get; private set; }
        public WeaponType Axe { get; private set; }
        public WeaponType Claymore { get; private set; }
        public WeaponType Trident { get; private set; }
        public WeaponType Flail { get; private set; }

        public WeaponModifier Stinging { get; private set; }
        public WeaponModifier Bloodthirsty { get; private set; }
        public WeaponModifier Sturdy { get; private set; }
        public WeaponModifier Unbreakable { get; private set; }
        public WeaponModifier Quality { get; private set; }
        public WeaponModifier Masterful { get; private set; }
        public WeaponModifier Worn { get; private set; }
        public WeaponModifier Tattered { get; private set; }
        public WeaponModifier Crystal { get; private set; }
        public WeaponModifier Spirit { get; private set; }
        public WeaponModifier Light { get; private set; }
        public WeaponModifier Heavy { get; private set; }

        public WeaponType GetWeaponType(string name)
        {
            return WeaponTypes.First(o => o.Name == name);
        }

        public bool WeaponTypeExists(string name)
        {
            return WeaponTypes.Any(o => o.Name == name);
        }

        public WeaponModifier GetWeaponModifier(string namePrefix)
        {
            return WeaponModifiers.First(o => o.NamePrefix == namePrefix);
        }

        public bool WeaponModifierExists(string namePrefix)
        {
            return WeaponModifiers.Any(o => o.NamePrefix == namePrefix);
        }

        public Monster NewMonster(ILocation level, Point position, int floor)
        {
            var modifier = Utilities.PassPercentileRoll(SpecialMonsterChance) ?
                Utilities.GetRandomFromList(MonsterModifiers) : null;

            return new Monster(level, position, MonsterTypes[LevelFactor(floor, MonsterTypes.Count())], modifier);
        }

        public Weapon NewWeapon(ILocation level, Point position, int floor)
        {
            var modifier = Utilities.PassPercentileRoll(SpecialWeaponChance) ?
                Utilities.GetRandomFromList(WeaponModifiers) : null;

            return new Weapon(level, position, WeaponTypes[LevelFactor(floor, WeaponTypes.Count())], modifier);
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
