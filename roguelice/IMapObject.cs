using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    interface IMapObject
    {
        DungeonLevel Location { get; set; }
        Point Position { get; set; }
        bool IsDead { get; set; }
        char Symbol { get; }
        string Overhead { get; }

        bool OnCollision(Player player);
        void Update(Player player);
    }

}
