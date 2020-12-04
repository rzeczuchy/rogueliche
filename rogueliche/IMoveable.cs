using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    interface IMoveable
    {
        ILocation Location { get; set; }
        Point Position { get; set; }
        bool Move(Point targetPosition);
    }
}
