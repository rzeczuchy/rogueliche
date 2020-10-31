using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class Tile
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

        public char Symbol
        {
            get
            {
                switch (Type)
                {
                    case TileType.floor:
                        return '.';
                    case TileType.wall:
                        return '\u2591';
                    case TileType.exit:
                        return 'E';
                    default:
                        return '?';
                }
            }
        }
    }
}
