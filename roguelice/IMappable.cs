using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    interface IMappable
    {
        DungeonLevel Location { get; set; }
        Point Position { get; set; }
        bool IsDead { get; }
        string Name { get; }
        char Symbol { get; }
        string Overhead { get; }

        void Update(Player player);
    }

}
