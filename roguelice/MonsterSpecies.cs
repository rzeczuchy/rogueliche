using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class MonsterSpecies
    {
        public MonsterSpecies(string name, char symbol, int maxHealth, int attack, int detectRange, int expGained)
        {
            Name = name;
            Symbol = symbol;
            MaxHealth = maxHealth;
            Attack = attack;
            DetectRange = detectRange;
            ExpGained = expGained;
        }

        public string Name { get; private set; }
        public char Symbol { get; private set; }
        public int MaxHealth { get; private set; }
        public int Attack { get; private set; }
        public int DetectRange { get; private set; }
        public int ExpGained { get; private set; }
    }
}
