using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class Save
    {
        public int PlayerKillCount { get; set; }
        public int PlayerBrokenWeapons { get; set; }
        public int PlayerMaxHealth { get; set; }
        public int PlayerHealth { get; set; }
        public int PlayerLvl { get; set; }
        public int PlayerExp { get; set; }
        public int PlayerExpToNextLvl { get; set; }
        public int PlayerExertion { get; set; }
        public int PlayerMaxExertion { get; set; }
    }
}
