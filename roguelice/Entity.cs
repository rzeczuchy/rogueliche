using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Entity : IMapObject
    {
        public Entity(DungeonLevel level, Point position)
        {
            Name = "undefined";
            Symbol = '?';
            Position = position;
            Location = level;
            if (Location != null && Position != null)
            {
                Location.Tilemap.ChangeObjectLocation(this, Location, Position);
            }
        }

        public bool IsDead { get; set; }
        public virtual string Name { get; set; }
        public virtual char Symbol { get; set; }
        public virtual string Overhead { get { return Name; } }
        public DungeonLevel Location { get; set; }
        public Point Position { get; set; }

        public virtual bool Move(Point targetPosition)
        {
            if (CanMoveToPosition(targetPosition))
            {
                if (CollidingEntity(targetPosition) == null)
                {
                    MoveToPosition(targetPosition);
                    return true;
                }
                else if (CollidingEntity(targetPosition) != null && this is Player player)
                {
                    return CollidingEntity(targetPosition).OnCollision(player);
                }
            }
            return false;
        }

        protected virtual void MoveToPosition(Point targetPosition)
        {
            Location.Tilemap.ChangeObjectPosition(this, targetPosition);
        }

        public bool CanMoveToPosition(Point targetPosition)
        {
            return Location.Tilemap.IsPositionWithinTilemap(targetPosition) && Location.Tilemap.IsWalkable(targetPosition);
        }

        public IMapObject CollidingEntity(Point targetPosition)
        {
            return Location.Tilemap.GetCreature(targetPosition);
        }

        public virtual bool OnCollision(Player player)
        {
            return false;
        }

        public virtual void Update(Player player)
        {
        }
    }
}
