using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Weapon : IMapObject
    {
        public Weapon(DungeonLevel level, Point position, WeaponType type, WeaponModifier modifier)
        {
            Type = type;
            Modifier = modifier;
            Durability = MaxDurability;
            Position = position;
            Location = level;
            if (Location != null && Position != null)
            {
                Location.Tilemap.ChangeItemLocation(this, Location, Position);
            }
        }

        public bool IsDead { get; set; }
        public string Name
        {
            get
            {
                if (Modifier != null)
                    return Modifier.NamePrefix + " " + Type.Name;
                else
                    return Type.Name;
            }
        }
        public string Overhead { get { string resulting = Name; if (IsBroken) resulting += " (broken)"; return resulting; } }
        public char Symbol { get { return Type.Symbol; } }
        public DungeonLevel Location { get; set; }
        public Point Position { get; set; }
        public WeaponType Type { get; private set; }
        public WeaponModifier Modifier { get; private set; }
        public int Durability { get; private set; }
        public bool IsBroken { get; private set; }

        public int Damage
        {
            get
            {
                if (IsBroken)
                    return 1;
                else
                {
                    if (Modifier != null)
                    {
                        double resulting = Type.Damage * Modifier.DamageMod;
                        return (int)resulting;
                    }
                    else
                        return Type.Damage;
                }
            }
        }

        public int StaminaCost
        {
            get
            {
                if (Modifier != null)
                {
                    double resulting = Type.StaminaCost * Modifier.StaminaCostMod;
                    return (int)resulting;
                }
                else
                    return Type.StaminaCost;
            }
        }

        public int MaxDurability
        {
            get
            {
                if (Modifier != null)
                {
                    double resulting = Type.MaxDurability * Modifier.MaxDurabilityMod;
                    return (int)resulting;
                }
                else
                    return Type.MaxDurability;
            }
        }

        public void DecrDurability(int amount, Player player)
        {
            if (!IsBroken)
            {
                Durability -= amount;
                if (Durability <= 0)
                {
                    Durability = 0;
                    IsBroken = true;
                    player.WeaponBroke();
                }
            }

        }

        public bool OnCollision(Player player)
        {
            return false;
        }

        public void Update(Player player)
        {
        }
    }
}
