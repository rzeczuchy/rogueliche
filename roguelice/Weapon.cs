using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class Weapon : IMappable
    {
        public Weapon(ILocation location, Point position, WeaponType type, WeaponModifier modifier)
        {
            if (location != null)
            {
                Layer = location.Tilemap.Items;
                Layer.ChangeLocation(this, location, position);
            }

            Type = type;
            Modifier = modifier;
            Durability = MaxDurability;
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
        public ILocation Location { get; set; }
        public TilemapLayer Layer { get; }
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
