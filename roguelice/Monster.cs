using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class Monster : IFightable, IMoveable, IMappable, ICollidable
    {
        public Monster(ILocation location, Point position, MonsterSpecies species, MonsterModifier modifier)
        {
            Species = species;
            Modifier = modifier;
            Health = MaxHealth;

            if (location != null && position != null)
            {
                location.Tilemap.ChangeObjectLocation(this, location, position);
            }
        }

        public MonsterSpecies Species { get; private set; }
        public MonsterModifier Modifier { get; private set; }
        public ILocation Location { get; set; }
        public Point Position { get; set; }
        public bool IsDead { get; set; }
        public string Overhead { get { return Name; } }
        public string Name { get { return Modifier != null ? Modifier.NamePrefix + " " + Species.Name : Species.Name; } }
        public char Symbol { get { return Species.Symbol; } }
        public bool IsSpooked { get; set; }
        public int MaxHealth
        {
            get
            {
                if (Modifier != null)
                {
                    double resulting = Species.MaxHealth * Modifier.MaxHealthMod;
                    return (int)resulting;
                }
                else
                    return Species.MaxHealth;
            }
        }
        public int Attack
        {
            get
            {
                if (Modifier != null)
                {
                    double resulting = Species.Attack * Modifier.AttackMod;
                    return (int)resulting;
                }
                else
                    return Species.Attack;
            }
        }
        public int DetectRange
        {
            get
            {
                if (Modifier != null)
                {
                    double resulting = Species.DetectRange * Modifier.DetectRangeMod;
                    return (int)resulting;
                }
                else
                    return Species.DetectRange;
            }
        }
        public int ExpGained
        {
            get
            {
                if (Modifier != null)
                {
                    double resulting = Species.ExpGained * Modifier.ExpGainedMod;
                    return (int)resulting;
                }
                else
                    return Species.ExpGained;
            }
        }
        public virtual int Health { get; set; }
        public Weapon CurrentWeapon { get; set; }

        public void ChangeHealth(int healthChange)
        {
            if (healthChange != 0)
            {
                Health += healthChange;

                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                }
                else if (Health <= 0)
                {
                    Health = 0;
                }
            }
        }

        public bool Move(Point targetPosition)
        {
            if (CanMoveToPosition(targetPosition))
            {
                if (CollidingEntity(targetPosition) == null)
                {
                    Location.Tilemap.ChangeObjectPosition(this, targetPosition);
                    return true;
                }
            }
            return false;
        }

        public bool CanMoveToPosition(Point targetPosition)
        {
            return Location.Tilemap.IsPositionWithinTilemap(targetPosition) && Location.Tilemap.IsWalkable(targetPosition);
        }

        public IMappable CollidingEntity(Point targetPosition)
        {
            return Location.Tilemap.GetCreature(targetPosition);
        }

        public void Update(Player player)
        {
            MonsterAI.Behave(this, player);
        }

        public bool OnCollision(Player player)
        {
            player.Hit(this);
            return true;
        }

        public void GainExp(int amount)
        {
        }

        public void CheckLevelUp()
        {
        }

        public void LevelUp()
        {
        }

        public void Die(IFightable attacker)
        {
            IsDead = true;
            Location.Tilemap.RemoveCreature(this);
        }

        public void Kill(IFightable target)
        {
        }
    }
}
