using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Tile
    {
        public Tile(TileType type)
        {
            Type = type;
        }

        public enum TileType
        {
            floor,
            wall,
            exit
        }

        public TileType Type { get; set; }
    }
}
