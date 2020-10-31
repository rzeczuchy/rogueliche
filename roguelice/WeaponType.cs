using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class WeaponType
    {
        public WeaponType(string name, char symbol, int damage, int staminaCost, int maxDurability)
        {
            Name = name;
            Symbol = symbol;
            Damage = damage;
            StaminaCost = staminaCost;
            MaxDurability = maxDurability;
        }

        public string Name { get; private set; }
        public char Symbol { get; private set; }
        public int Damage { get; private set; }
        public int StaminaCost { get; private set; }
        public int MaxDurability { get; private set; }
    }
}
