using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class TilemapLayer
    {
        private readonly Tilemap tilemap;
        private readonly IMappable[,] mappables;

        public TilemapLayer(Tilemap tilemap)
        {
            if (tilemap != null)
            {
                this.tilemap = tilemap;
                mappables = new IMappable[tilemap.Width, tilemap.Height];
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public IMappable GetMappable(Point position)
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

        public void SetMappable(IMappable mappable, Point position)
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
            IMappable o = GetMappable(pos);
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
            SetMappable(null, o.Position);
        }
    }
}
