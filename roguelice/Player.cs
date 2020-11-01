using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class Player : IFightable, IMoveable, IMappable
    {
        private const int LevelCap = 111;
        private const int LevelIncreaseFactor = 11;
        private int staminaRegen;
        private bool endTurn;

        public Player(ILocation location, Point position)
        {
            if (location != null)
            {
                Layer = location.Tilemap.Creatures;
                Layer.ChangeLocation(this, location, position);
                location.Tilemap.UpdateFogOfWar(this);
                location.Tilemap.UpdateFieldOfVisibility(this);
            }

            Name = "hero";
            Symbol = '@';
            MaxHealth = 50;
            Health = MaxHealth;
            MaxExertion = 50;
            Lvl = 1;
            Exp = 0;
            ExpToNextLvl = 100;

            StartingWeapon = new WeaponType("dagger", 'D', 3, 3, 20);
            CurrentWeapon = new Weapon(null, null, StartingWeapon, null);
        }

        public string Name { get; }
        public char Symbol { get; }
        public ILocation Location { get; set; }
        public TilemapLayer Layer { get; }
        public Point Position { get; set; }
        public bool IsDead { get; set; }
        public string Overhead { get { return Name; } }
        public int KillCount { get; private set; }
        public int BrokenWeapons { get; private set; }
        public WeaponType StartingWeapon { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public Weapon CurrentWeapon { get; private set; }
        public int Attack { get { return CurrentWeapon.Damage; } }
        public int Lvl { get; private set; }
        public int Exp { get; private set; }
        public int ExpToNextLvl { get; private set; }
        public int Exertion { get; private set; }
        public int MaxExertion { get; private set; }
        public int ExpGained { get; set; }

        public void GainExp(int amount)
        {
            Exp += amount;
            CheckLevelUp();
        }

        public void CheckLevelUp()
        {
            if (Exp >= ExpToNextLvl)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            if (Lvl < LevelCap)
            {
                Lvl++;
                Exp -= ExpToNextLvl;
                ExpToNextLvl += ExpToNextLvl / LevelIncreaseFactor;
                MaxHealth++;
                Health = MaxHealth;
                MaxExertion++;
            }
        }

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

        public void Update(ConsoleKey input, UI ui, Dungeon dungeon)
        {
            if (!IsDead)
            {
                HandleInput(input, ui, dungeon);

                if (endTurn == true)
                {
                    EndTurn();
                    endTurn = false;
                }
            }
        }

        // Update function called for objects in location on each turn.
        public void Update(Player player)
        {
        }

        public void HandleInput(ConsoleKey input, UI ui, Dungeon dungeon)
        {
            switch (input)
            {
                // MOVE UP
                case ConsoleKey.UpArrow:
                case ConsoleKey.NumPad8:
                    Move(new Point(Position.X, Position.Y - 1));
                    break;
                // MOVE DOWN
                case ConsoleKey.DownArrow:
                case ConsoleKey.NumPad2:
                    Move(new Point(Position.X, Position.Y + 1));
                    break;
                // MOVE LEFT
                case ConsoleKey.LeftArrow:
                case ConsoleKey.NumPad4:
                    Move(new Point(Position.X - 1, Position.Y));
                    break;
                // MOVE RIGHT
                case ConsoleKey.RightArrow:
                case ConsoleKey.NumPad6:
                    Move(new Point(Position.X + 1, Position.Y));
                    break;
                // MOVE UP-LEFT
                case ConsoleKey.NumPad7:
                    Move(new Point(Position.X - 1, Position.Y - 1));
                    break;
                // MOVE UP-RIGHT
                case ConsoleKey.NumPad9:
                    Move(new Point(Position.X + 1, Position.Y - 1));
                    break;
                // MOVE DOWN-LEFT
                case ConsoleKey.NumPad1:
                    Move(new Point(Position.X - 1, Position.Y + 1));
                    break;
                // MOVE DOWN-RIGHT
                case ConsoleKey.NumPad3:
                    Move(new Point(Position.X + 1, Position.Y + 1));
                    break;
                // WAIT
                case ConsoleKey.NumPad5:
                case ConsoleKey.Spacebar:
                case ConsoleKey.W:
                    Wait();
                    break;
                // USE EXIT
                case ConsoleKey.Enter:
                case ConsoleKey.E:
                    EnterNextLevel(dungeon);
                    break;
                // SWITCH WEAPON
                case ConsoleKey.S:
                case ConsoleKey.NumPad0:
                    SwitchWeapon();
                    break;
                // TOGGLE OVERHEAD DISPLAY
                case ConsoleKey.Tab:
                case ConsoleKey.Divide:
                    ui.ToggleOverheads();
                    break;
                default:
                    break;
            }

        }

        public bool Move(Point targetPosition)
        {
            if (CanMoveToPosition(targetPosition))
            {
                IMappable entityAtTargetPosition = CollidingEntity(targetPosition);
                endTurn = true;
                if (entityAtTargetPosition == null)
                {
                    staminaRegen = 0;
                    Layer.ChangePosition(this, targetPosition);
                    return true;
                }
                else if (entityAtTargetPosition != null && entityAtTargetPosition is ICollidable collidable)
                {
                    return collidable.OnCollision(this);
                }
            }
            return false;
        }

        public void Hit(Monster monster)
        {
            CombatSystem.Hit(this, monster);
            staminaRegen = 0;
            AddExertion(CurrentWeapon.StaminaCost);
        }

        public void Die()
        {
            IsDead = true;
        }

        public void Die(IFightable attacker)
        {
            Die();
        }

        public void Kill(IFightable target)
        {
            GainExp(target.ExpGained);
            KillCount++;
        }

        public void WeaponBroke()
        {
            BrokenWeapons++;
        }
        
        private void AddExertion(int amount)
        {
            Exertion += amount;

            if (Exertion > MaxExertion)
            {
                int healthLoss = Exertion - MaxExertion;
                ChangeHealth(-healthLoss);
                Exertion = MaxExertion;

                if (Health <= 0)
                {
                    Die();
                }
            }
        }

        private bool CanMoveToPosition(Point targetPosition)
        {
            return Location.Tilemap.ContainsPosition(targetPosition) && Location.Tilemap.IsWalkable(targetPosition);
        }

        private IMappable CollidingEntity(Point targetPosition)
        {
            return Layer.Get(targetPosition);
        }

        private void EndTurn()
        {
            Location.Tilemap.Update(this);
        }

        private void EnterNextLevel(Dungeon dungeon)
        {
            if (Location.Tilemap.GetTile(Position).Type == Tile.TileType.exit)
            {
                if (Location is DungeonLevel level)
                {
                    level.GoDownOneLevel(this);
                }
            }
        }
        
        private void RegenerateStamina()
        {
            if (Exertion > 0)
            {
                Exertion -= staminaRegen;
            }
            if (Exertion <= 0)
            {
                Exertion = 0;
                staminaRegen = 0;
            }
        }

        private void SwitchWeapon()
        {
            var item = Location.Tilemap.Items.Get(Position);

            if (item is Weapon weapon)
            {
                Weapon previousWeapon = CurrentWeapon;
                CurrentWeapon = weapon;
                Location.Tilemap.Items.Set(previousWeapon, Position);
                endTurn = true;
            }
        }

        private void Wait()
        {
            staminaRegen++;
            RegenerateStamina();
            endTurn = true;
        }
    }
}
