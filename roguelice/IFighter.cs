using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    interface IFighter : IMapObject, IMoveable
    {
        string Name { get; }
        Weapon CurrentWeapon { get; }
        int Health { get; }
        int MaxHealth { get; }
        int Attack { get; }
        int ExpGained { get; }

        void ChangeHealth(int amount);
        void GainExp(int amount);
        void CheckLevelUp();
        void LevelUp();
        void Die(IFighter attacker);
        void Kill(IFighter target);
    }

}
