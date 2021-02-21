using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class HealingPlant : IMappable, ICollidable
    {
        private const int RestoreHealth = 10;
        private TilemapLayer _layer;

        public HealingPlant(ILocation location, Point position)
        {
            if (location != null)
            {
                _layer = location.Tilemap.Creatures;
                Place(location, position);
            }

            Name = "healing plant";
            Symbol = '\u2663';
        }

        public string Name { get; }
        public char Symbol { get; }
        public ILocation Location { get; set; }
        public Point Position { get; set; }
        public bool IsDead { get; set; }
        public string Overhead { get { return Name; } }

        public bool OnCollision(Player player)
        {
            player.Health += RestoreHealth;
            _layer.Remove(this);
            return true;
        }

        public void Place(ILocation targetLocation, Point targetPos)
        {
            _layer = targetLocation.Tilemap.Creatures;
            _layer.Set(this, targetPos);
            Location = targetLocation;
            Position = targetPos;
        }

        public void Remove()
        {
            _layer.Remove(this);
            Location = null;
            Position = null;
        }

        public void Update(Player player)
        {
        }
    }
}
