using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class MonsterModifier
    {
        public MonsterModifier(string prefix, double health, double attack, double detect, double exp)
        {
            NamePrefix = prefix;
            MaxHealthMod = health;
            AttackMod = attack;
            DetectRangeMod = detect;
            ExpGainedMod = exp;
        }

        public string NamePrefix { get; private set; }
        public double MaxHealthMod { get; private set; }
        public double AttackMod { get; private set; }
        public double DetectRangeMod { get; private set; }
        public double ExpGainedMod { get; private set; }
    }
}
