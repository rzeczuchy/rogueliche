﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    class CombatSystem
    {
        public static void Hit(IFightable attacker, IFightable target)
        {
            InflictDamage(target, CalculateDamage(attacker, target));
            DamageWeapon(attacker);
            CheckIfDead(attacker, target);
        }

        public static int GetMinDamage(IFightable fighter)
        {
            return fighter.Attack / 2;
        }

        public static int GetMaxDamage(IFightable fighter)
        {
            return fighter.Attack;
        }

        private static int CalculateDamage(IFightable attacker, IFightable target)
        {
            int resulting = Utilities.RandomNumber(GetMinDamage(attacker), GetMaxDamage(attacker));
            return resulting > 0 ? resulting : 0;
        }

        private static void InflictDamage(IFightable target, int damage)
        {
            target.Health -= damage;
        }

        private static void DamageWeapon(IFightable attacker)
        {
            if (attacker is Player player)
            {
                int chanceOfDamage = 70;// percentile
                if (Utilities.PassPercentileRoll(chanceOfDamage))
                {
                    player.CurrentWeapon.DecreaseDurability(1, player);
                }
            }
        }

        private static void CheckIfDead(IFightable attacker, IFightable target)
        {
            if (target.Health <= 0)
            {
                target.Die(attacker);
                attacker.Kill(target);
            }
        }
    }
}
