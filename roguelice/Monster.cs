using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Monster : Entity, IFighter
    {
        public Monster(DungeonLevel level, Point position, MonsterSpecies species, MonsterModifier modifier) : base(level, position)
        {
            Species = species;
            Modifier = modifier;
            Health = MaxHealth;
        }

        public MonsterSpecies Species { get; private set; }
        public MonsterModifier Modifier { get; private set; }
        public override string Overhead { get { return Name; } }
        public override string Name { get { return Modifier != null ? Modifier.NamePrefix + " " + Species.Name : Species.Name; } }
        public override char Symbol { get { return Species.Symbol; } }
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

        public override void Update(Player player)
        {
            MonsterAI.Behave(this, player);
        }

        public override bool OnCollision(Player player)
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

        public void Die(IFighter attacker)
        {
            IsDead = true;
            Location.RemoveCreature(this);
        }

        public void Kill(IFighter target)
        {
        }
    }
}
