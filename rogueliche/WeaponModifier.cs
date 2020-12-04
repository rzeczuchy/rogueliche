using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class WeaponModifier
    {
        public WeaponModifier(string prefix, double damage, double stamCost, double maxDurab)
        {
            NamePrefix = prefix;
            DamageMod = damage;
            StaminaCostMod = stamCost;
            MaxDurabilityMod = maxDurab;
        }

        public string NamePrefix { get; private set; }
        public double DamageMod { get; private set; }
        public double StaminaCostMod { get; private set; }
        public double MaxDurabilityMod { get; private set; }
    }
}
