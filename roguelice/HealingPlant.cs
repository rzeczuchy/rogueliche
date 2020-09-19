using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class HealingPlant : Entity
    {
        private const int RestoreHealth = 10;

        public HealingPlant(DungeonLevel level, Point position) : base(level, position)
        {
            Name = "healing plant";
            Symbol = '\u2663';
        }

        public override bool OnCollision(Player player)
        {
            player.ChangeHealth(RestoreHealth);
            Location.RemoveCreature(this);
            return true;
        }
    }
}
