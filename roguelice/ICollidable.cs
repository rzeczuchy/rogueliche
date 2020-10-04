using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    interface ICollidable
    {
        bool OnCollision(Player player);
    }
}
