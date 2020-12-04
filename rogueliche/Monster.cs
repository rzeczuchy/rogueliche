﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class Monster : IFightable, IMoveable, IMappable, ICollidable
    {
        private int _health;
        private TilemapLayer _layer;

        public Monster(ILocation location, Point position, MonsterSpecies species, MonsterModifier modifier)
        {
            if (location != null)
            {
                _layer = location.Tilemap.Creatures;
                Place(location, position);
            }

            Species = species;
            Modifier = modifier;
            Health = MaxHealth;
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
        public virtual int Health
        {
            get => _health;
            set => _health = Numbers.Clamp(value, 0, MaxHealth);
        }
        public Weapon CurrentWeapon { get; set; }

        public bool Move(Point targetPosition)
        {
            var targetLocation = Location;

            if (CanMoveToPosition(targetPosition))
            {
                if (CollidingEntity(targetPosition) == null)
                {
                    Remove();
                    Place(targetLocation, targetPosition);
                    return true;
                }
            }
            return false;
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

        public bool CanMoveToPosition(Point targetPosition)
        {
            return Location.Tilemap.ContainsPosition(targetPosition) && Location.Tilemap.IsWalkable(targetPosition);
        }

        public IMappable CollidingEntity(Point targetPosition)
        {
            return _layer.Get(targetPosition);
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
            _layer.Remove(this);
        }

        public void Kill(IFightable target)
        {
        }
    }
}