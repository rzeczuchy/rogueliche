using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    interface IMoveable : IMapObject
    {
        bool Move(Point targetPosition);
    }
}
