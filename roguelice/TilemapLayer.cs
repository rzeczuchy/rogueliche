using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class TilemapLayer
    {
        private readonly Tilemap tilemap;
        private readonly IMappable[,] mappables;

        public TilemapLayer(Tilemap tilemap)
        {
            this.tilemap = tilemap;
            mappables = new IMappable[tilemap.Width, tilemap.Height];
        }

        public IMappable Get(Point position)
        {
            if (tilemap.ContainsPosition(position))
            {
                return mappables[position.X, position.Y];
            }
            else
            {
                return null;
            }
        }

        public void Set(IMappable mappable, Point position)
        {
            if (tilemap.ContainsPosition(position))
            {
                mappables[position.X, position.Y] = mappable;
            }
        }

        public void FilterDead()
        {
            tilemap.PerformOnAllTiles((pos) => RemoveDeatAtPosition(pos));
        }

        public void RemoveDeatAtPosition(Point pos)
        {
            IMappable o = Get(pos);
            if (o != null)
            {
                if (o.IsDead)
                {
                    Remove(o);
                }
                else
                {
                    tilemap.ToUpdate.Add(o);
                }
            }
        }

        public void Remove(IMappable o)
        {
            Set(null, o.Position);
        }
    }
}
