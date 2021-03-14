using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    class CellularAutomata
    {
        public static void ErodeTiles(Tilemap tilemap)
        {
            tilemap.PerformOnAllTiles((pos) => SwitchTileType(tilemap, pos));
        }

        public static void ErodeTiles(Tilemap tilemap, Rectangle bounds)
        {
            tilemap.PerformOnTiles((pos) => SwitchTileType(tilemap, pos), bounds);
        }

        public static void ErodeTiles(Tilemap tilemap, int left, int top, int width, int height)
        {
            tilemap.PerformOnTiles((pos) => SwitchTileType(tilemap, pos), new Rectangle(left, top, width, height));
        }

        public static void SwitchTileType(Tilemap tilemap, Point tilePosition)
        {
            if (NumberOfFloorNeighbours(tilemap, tilePosition) > 4)
            {
                tilemap.SetTile(tilePosition, Tile.TileType.floor);
            }
            else
            {
                tilemap.SetTile(tilePosition, Tile.TileType.wall);
            }
        }

        public static int NumberOfFloorNeighbours(Tilemap tilemap, Point pos)
        {
            var neighbours = new List<Tile>();

            for (int x = pos.X - 1; x <= pos.X + 1; x++)
                for (int y = pos.Y - 1; y <= pos.Y + 1; y++)
                {
                    if (tilemap.ContainsPosition(new Point(x, y)))
                    {
                        neighbours.Add(tilemap.GetTile(new Point(x, y)));
                    }
                }

            return neighbours.Where(i => i.Type == Tile.TileType.floor).Count();
        }
    }
}

