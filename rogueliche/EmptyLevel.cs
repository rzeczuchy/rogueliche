using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class EmptyLevel : ILocation
    {
        public EmptyLevel(string name, int width, int height)
        {
            Name = name;
            Bounds = new Rectangle(0, 0, width, height);
            Tilemap = new Tilemap(this);
        }

        public Generator Generator { get => null; }
        public Point Entrance { get => null; }
        public Point Exit { get => null; }
        public string Name { get; private set; }
        public string Id { get; private set; }
        public Tilemap Tilemap { get; private set; }
        public Rectangle Bounds { get; private set; }

        public void UpdateObjects(Player player)
        {
            Tilemap.Update(player);
        }

        private void GenerateLevel()
        {
            Tilemap.FillWithTile(Bounds, Tile.TileType.wall);
        }
    }
}
