﻿using System;
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
            tilemap.PerformOnAllTiles((pos) => FilterDeatAtPosition(pos));
        }

        public void FilterDeatAtPosition(Point pos)
        {
            IMappable o = GetMappable(pos);
            if (o != null)
            {
                if (o.IsDead)
                {
                    RemoveMappable(o);
                }
                else
                {
                    tilemap.ToUpdate.Add(o);
                }
            }
        }

        public void RemoveMappable(IMappable o)
        {
            SetMappable(null, o.Position);
        }
    }
}
