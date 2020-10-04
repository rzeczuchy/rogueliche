using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class DungeonLevel
    {
        private readonly Dungeon dungeon;
        private readonly List<IMappable> toUpdate;

        public DungeonLevel(Dungeon dungeon, int levelIndex)
        {
            this.dungeon = dungeon;
            LevelIndex = levelIndex;
            Tilemap = new Tilemap(this, dungeon.Width, dungeon.Height);
            Bounds = new Rectangle(0, 0, dungeon.Width, dungeon.Height);
            toUpdate = new List<IMappable>();

            GenerateLevel();
        }

        public ChamberTree ChamberTree { get; private set; }
        public int LevelIndex { get; private set; }
        public Point Entrance { get; private set; }
        public Point Exit { get; private set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public Tilemap Tilemap { get; private set; }
        public Rectangle Bounds { get; private set; }

        public Point RandomPosition(Rectangle rect)
        {
            return new Point(Numbers.RandomNumber(rect.Left, rect.Right), Numbers.RandomNumber(rect.Top, rect.Bottom));
        }

        public void UpdateObjects(Player player)
        {
            toUpdate.Clear();

            for (int y = 0; y < Tilemap.Height; y++)
            {
                for (int x = 0; x < Tilemap.Width; x++)
                {
                    AddRemoveCreature(y, x);
                    AddRemoveItem(y, x);
                }
            }

            for (int u = 0; u < toUpdate.Count(); u++)
            {
                if (!toUpdate[u].IsDead)
                {
                    toUpdate[u].Update(player);
                }
            }
        }

        private void AddRemoveItem(int y, int x)
        {
            IMappable o = Tilemap.GetItem(new Point(x, y));
            if (o != null)
            {
                if (o.IsDead)
                {
                    RemoveItem(o);
                }
                else
                {
                    toUpdate.Add(o);
                }
            }
        }

        public void RemoveItem(IMappable o)
        {
            Tilemap.SetItem(null, new Point(o.Position.X, o.Position.Y));
        }

        private void AddRemoveCreature(int y, int x)
        {
            IMappable o = Tilemap.GetCreature(new Point(x, y));
            if (o != null)
            {
                if (o.IsDead)
                {
                    RemoveCreature(o);
                }
                else
                {
                    toUpdate.Add(o);
                }
            }
        }

        public void RemoveCreature(IMappable o)
        {
            Tilemap.SetCreature(null, new Point(o.Position.X, o.Position.Y));
        }

        private void GenerateLevel()
        {
            Tilemap.FillWithType(Bounds, Tile.TileType.wall);
            ChamberTree = new ChamberTree(Bounds.Size, dungeon.MinRooms, dungeon.MaxRooms, dungeon.MinRoomWidth, dungeon.MaxRoomWidth,
                dungeon.MinRoomHeight, dungeon.MaxRoomHeight, dungeon.ForceRegularRooms);

            Entrance = new Point(ChamberTree.StartingChamber.Center.X, ChamberTree.StartingChamber.Center.Y);

            ChamberTree.FillChambersWithTile(Tile.TileType.floor, Tilemap);
            ChamberTree.FillPassagesWithTile(Tile.TileType.floor, Tilemap);

            if (Numbers.PassPercentileRoll(dungeon.CaveChance))
            {
                CellularAutomata.ErodeTiles(Tilemap);
            }

            PlaceObject(TryPlaceStairsDown, 1);

            int monsters = (int)(dungeon.MonstersPerRoom * ChamberTree.Chambers.Count);
            PlaceObject(TryPlaceMonster, monsters);

            int weapons = (int)(dungeon.WeaponsPerRoom * ChamberTree.Chambers.Count);
            PlaceObject(TryPlaceItem, weapons);

            int plants = (int)(dungeon.PlantsPerRoom * ChamberTree.Chambers.Count);
            PlaceObject(TryPlacePlant, plants);
        }

        void PlaceObject(Func<bool> tryPlace, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                bool objectPlaced = false;
                int attempts = 0;
                while (!objectPlaced && attempts < 30)
                {
                    objectPlaced = tryPlace();
                    attempts++;
                }
            }
        }

        private bool TryPlaceStairsDown()
        {
            Point pos = ChamberTree.GetRandomChamber().Center;
            Tile tile = Tilemap.GetTile(pos);

            if (tile.Type != Tile.TileType.wall)
            {
                Tilemap.SetTile(pos, Tile.TileType.exit);
                Exit = new Point(pos.X, pos.Y);
                return true;
            }
            return false;
        }

        private bool TryPlaceMonster()
        {
            Point pos = RandomPosition(ChamberTree.GetRandomChamber());

            if (CanPlaceCreature(pos))
            {
                dungeon.Generator.NewMonster(this, pos, LevelIndex);
                return true;
            }
            return false;
        }

        private bool TryPlaceItem()
        {
            Point pos = RandomPosition(ChamberTree.GetRandomChamber());

            if (CanPlaceItem(pos))
            {
                dungeon.Generator.NewWeapon(this, pos, LevelIndex);
                return true;
            }
            return false;
        }

        private bool TryPlacePlant()
        {
            Point pos = RandomPosition(ChamberTree.GetRandomChamber());

            if (CanPlaceCreature(pos))
            {
                new HealingPlant(this, pos);
                return true;
            }
            return false;
        }

        bool CanPlaceCreature(Point pos)
        {
            return Tilemap.IsPositionWithinTilemap(pos) && Tilemap.IsWalkable(pos) &&
                Tilemap.GetCreature(pos) == null && Tilemap.GetTile(pos).Type != Tile.TileType.exit;
        }

        bool CanPlaceItem(Point pos)
        {
            return Tilemap.IsPositionWithinTilemap(pos) && Tilemap.IsWalkable(pos) &&
                (Tilemap.GetCreature(pos) == null || Tilemap.GetCreature(pos) is Monster) &&
                Tilemap.GetTile(pos).Type != Tile.TileType.exit;
        }

        public void GoDownOneLevel(Player player)
        {
            DungeonLevel below = dungeon.NewLevel();

            Tilemap.ChangeObjectLocation(player, below, new Point(below.Entrance.X + 1, below.Entrance.Y));
        }
    }
}
