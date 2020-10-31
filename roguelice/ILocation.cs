using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public interface ILocation
    {
        Point Entrance { get; }
        Point Exit { get; }
        string Name { get; }
        string Id { get; }
        Tilemap Tilemap { get; }
        Rectangle Bounds { get; }
    }
}
