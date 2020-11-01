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
                Layer.ChangeLocation(this, location, position);
            }

            Name = "healing plant";
            Symbol = '\u2663';
        }

        public string Name { get; }
        public char Symbol { get; }
        public ILocation Location { get; set; }
        public TilemapLayer Layer { get; }
        public Point Position { get; set; }
        public bool IsDead { get; set; }
        public string Overhead { get { return Name; } }

        public bool OnCollision(Player player)
        {
            player.ChangeHealth(RestoreHealth);
            Layer.Remove(this);
            return true;
        }

        public void Update(Player player)
        {
        }
    }
}
