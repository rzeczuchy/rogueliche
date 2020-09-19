using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class CellularAutomata
    {
        public static void ErodeTiles(Tilemap tilemap, int left, int top, int width, int height)
        {
            for (int ypos = top; ypos < top + height; ypos++)
                for (int xpos = left; xpos <= left + width; xpos++)
                {
                    Point tilePosition = new Point(xpos, ypos);

                    if (tilemap.IsPositionWithinTilemap(tilePosition))
                    {
                        SwitchTileType(tilemap, tilePosition);
                    }
                }
        }

        public static void ErodeTiles(Tilemap tilemap)
        {
            ErodeTiles(tilemap, 0, 0, tilemap.Width, tilemap.Height);
        }

        private static void SwitchTileType(Tilemap tilemap, Point tilePosition)
        {
            if (NumberOfFloorNeighbors(tilemap, tilePosition) > 4)
            {
                tilemap.SetTile(tilePosition, Tile.TileType.floor);
            }
            else
            {
                tilemap.SetTile(tilePosition, Tile.TileType.wall);
            }
        }

        private static int NumberOfFloorNeighbors(Tilemap tilemap, Point pos)
        {
            var neighbors = new List<Tile>();

            for (int x = pos.X - 1; x <= pos.X + 1; x++)
                for (int y = pos.Y - 1; y <= pos.Y + 1; y++)
                {
                    if (tilemap.IsPositionWithinTilemap(new Point(x, y)))
                    {
                        neighbors.Add(tilemap.GetTile(new Point(x, y)));
                    }
                }

            return neighbors.Where(i => i.Type == Tile.TileType.floor).Count();
        }
    }
}

