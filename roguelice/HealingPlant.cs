using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class HealingPlant : IMappable, ICollidable
    {
        private const int RestoreHealth = 10;

        public HealingPlant(ILocation location, Point position)
        {
            if (location != null)
            {
                Layer = location.Tilemap.Creatures;
                ChangeLocation(location, position);
            }

            Name = "healing plant";
            Symbol = '\u2663';
        }

        public string Name { get; }
        public char Symbol { get; }
        public ILocation Location { get; set; }
        public TilemapLayer Layer { get; set; }
        public Point Position { get; set; }
        public bool IsDead { get; set; }
        public string Overhead { get { return Name; } }

        public bool OnCollision(Player player)
        {
            player.ChangeHealth(RestoreHealth);
            Layer.Remove(this);
            return true;
        }

        public void ChangePosition(Point targetPosition)
        {
            Layer.Remove(this);
            Layer.Set(this, targetPosition);
            Position = targetPosition;
        }

        public void ChangeLocation(ILocation targetLocation, Point targetPosition)
        {
            Layer.Remove(this);
            Layer = targetLocation.Tilemap.Creatures;
            Layer.Set(this, targetPosition);
            Location = targetLocation;
            Position = targetPosition;
        }

        public void Update(Player player)
        {
        }
    }
}
