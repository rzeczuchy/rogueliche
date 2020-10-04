using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    static class MonsterAI
    {
        public static void Behave(Monster monster, Player player)
        {
            if (!monster.IsDead && !player.IsDead && monster.Position != null && player.Position != null)
            {
                TestIfSpooked(monster);

                if (SeesPlayer(monster, player))
                {
                    if (monster.IsSpooked)
                    {
                        RunFromPlayer(monster, player);
                    }
                    else
                    {
                        AttackPlayer(monster, player);
                    }
                }
                else
                {
                    WanderAround(monster);
                }
            }
        }

        static void TestIfSpooked(Monster monster)
        {
            if (AI.HasLowHealth(monster) && !monster.IsSpooked && Numbers.PassPercentileRoll(20))
            {
                monster.IsSpooked = true;
            }
            else if (!AI.HasLowHealth(monster) && monster.IsSpooked && Numbers.PassPercentileRoll(80))
            {
                monster.IsSpooked = false;
            }
        }

        static void WanderAround(Monster monster)
        {
            {
                AI.MoveAtRandom(monster);
            }
        }

        static void AttackPlayer(Monster monster, Player player)
        {
            if (AI.AreNeighboring(monster, player))
            {
                CombatSystem.Hit(monster, player);
            }
            else
            {
                AI.MoveTowards(monster, player);
            }
        }

        static void RunFromPlayer(Monster monster, Player player)
        {
            if (AI.CanMove(monster))
            {
                AI.MoveAwayFrom(monster, player);
            }
            else if (AI.AreNeighboring(monster, player))
            {
                CombatSystem.Hit(monster, player);
            }
            else
            {
                // stand still
            }
        }

        static bool SeesPlayer(Monster monster, Player player)
        {
            return Point.Distance(monster.Position, player.Position)
                <= monster.DetectRange;
        }
    }
}
