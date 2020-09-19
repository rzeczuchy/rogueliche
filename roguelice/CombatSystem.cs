using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class CombatSystem
    {
        public static void Hit(IFighter attacker, IFighter target)
        {
            InflictDamage(attacker.Name, target, CalculateDamage(attacker, target));
            DamageWeapon(attacker);
            CheckIfDead(attacker, target);
        }

        public static int GetMinDamage(IFighter fighter)
        {
            return fighter.Attack / 2;
        }

        public static int GetMaxDamage(IFighter fighter)
        {
            return fighter.Attack;
        }

        private static int CalculateDamage(IFighter attacker, IFighter target)
        {
            int resulting = Numbers.RandomNumber(GetMinDamage(attacker), GetMaxDamage(attacker));
            return resulting > 0 ? resulting : 0;
        }

        private static void InflictDamage(string source, IFighter target, int damage)
        {
            target.ChangeHealth(-damage);
        }

        private static void DamageWeapon(IFighter attacker)
        {
            if (attacker is Player player)
            {
                int chanceOfDamage = 70;// percentile
                if (Numbers.PassPercentileRoll(chanceOfDamage))
                {
                    player.CurrentWeapon.DecrDurability(1, player);
                }
            }
        }

        private static void CheckIfDead(IFighter attacker, IFighter target)
        {
            if (target.Health <= 0)
            {
                target.Die(attacker);
                attacker.Kill(target);
            }
        }
    }
}
