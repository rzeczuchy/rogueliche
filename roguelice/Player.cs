using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Player : Entity, IFighter
    {
        private const int LevelCap = 111;
        private const int LevelIncreaseFactor = 11;
        private int staminaRegen;
        private bool endTurn;

        public Player(DungeonLevel level, Point position) : base(level, position)
        {
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

        void RegenStamina()
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

        void AddExertion(int amount)
        {
            Exertion += amount;

            if (Exertion > MaxExertion)
            {
                int healthLoss = Exertion - MaxExertion;
                ChangeHealth(-healthLoss);
                Exertion = MaxExertion;
            }
        }

        public void Update(Game game, UI ui, Dungeon dungeon)
        {
            if (!IsDead)
            {
                HandleInput(game, ui, dungeon);

                if (endTurn == true)
                {
                    EndTurn();
                    endTurn = false;
                }
            }
            else
                game.DisplayDeathScreen();
        }

        void Wait()
        {
            staminaRegen++;
            RegenStamina();
            endTurn = true;
        }

        public bool CanSee(int x, int y)
        {
            return Point.Distance(Position, new Point(x, y)) < 25;
        }

        public void HandleInput(Game game, UI ui, Dungeon dungeon)
        {
            bool validKey;
            do
            {
                validKey = true;
                ConsoleKey input = Console.ReadKey(true).Key;
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
                    // QUIT GAME
                    case ConsoleKey.Escape:
                        game.Close();
                        break;
                    default:
                        validKey = false;
                        break;
                }
            } while (!validKey);
        }

        void SwitchWeapon()
        {
            var item = Location.Tilemap.GetItem(Position);

            if (item != null && item is Weapon weapon)
            {
                Weapon previousWeapon = CurrentWeapon;
                CurrentWeapon = weapon;
                Location.Tilemap.SetItem(previousWeapon, Position);
                endTurn = true;
            }
        }

        public override bool Move(Point targetPosition)
        {
            if (CanMoveToPosition(targetPosition))
            {
                endTurn = true;
                if (CollidingEntity(targetPosition) == null)
                {
                    staminaRegen = 0;
                    MoveToPosition(targetPosition);
                    return true;
                }
                else if (CollidingEntity(targetPosition) != null && this is Player player)
                {
                    return CollidingEntity(targetPosition).OnCollision(player);
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

        void EndTurn()
        {
            Location.UpdateObjects(this);
        }

        void EnterNextLevel(Dungeon dungeon)
        {
            if (Location.Tilemap.GetTile(Position).Type == Tile.TileType.exit)
            {
                if (Location is DungeonLevel level)
                {
                    level.GoDownOneLevel(this);
                }
            }
        }

        public void Die(IFighter attacker)
        {
            IsDead = true;
        }

        public void Kill(IFighter target)
        {
            GainExp(target.ExpGained);
            KillCount++;
        }

        public void WeaponBroke()
        {
            BrokenWeapons++;
        }
    }
}
