using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class Monster : IFightable, IMoveable, IMappable, ICollidable
    {
        private int _health;

        public Monster(ILocation location, Point position, MonsterSpecies species, MonsterModifier modifier)
        {
            if (location != null)
            {
                Layer = location.Tilemap.Creatures;
                ChangeLocation(location, position);
            }

            Species = species;
            Modifier = modifier;
            Health = MaxHealth;
        }

        public MonsterSpecies Species { get; private set; }
        public MonsterModifier Modifier { get; private set; }
        public ILocation Location { get; set; }
        public TilemapLayer Layer { get; set; }
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
        public virtual int Health
        {
            get => _health;
            set => _health = Numbers.Clamp(value, 0, MaxHealth);
        }
        public Weapon CurrentWeapon { get; set; }

        public bool Move(Point targetPosition)
        {
            if (CanMoveToPosition(targetPosition))
            {
                if (CollidingEntity(targetPosition) == null)
                {
                    ChangePosition(targetPosition);
                    return true;
                }
            }
            return false;
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

        public bool CanMoveToPosition(Point targetPosition)
        {
            return Location.Tilemap.ContainsPosition(targetPosition) && Location.Tilemap.IsWalkable(targetPosition);
        }

        public IMappable CollidingEntity(Point targetPosition)
        {
            return Layer.Get(targetPosition);
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
            Layer.Remove(this);
        }

        public void Kill(IFightable target)
        {
        }
    }
}
