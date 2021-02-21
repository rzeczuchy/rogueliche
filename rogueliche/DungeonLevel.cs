using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class DungeonLevel : ILocation
    {
        private readonly Dungeon dungeon;

        public DungeonLevel(Dungeon dungeon)
        {
            if (dungeon != null)
            {
                this.dungeon = dungeon;
            }
            else
            {
                throw new ArgumentNullException();
            }

            LevelIndex = dungeon.LevelIndex;
            Name = "Dungeon -" + dungeon.LevelIndex;
            Bounds = new Rectangle(0, 0, dungeon.Width, dungeon.Height);
            Tilemap = new Tilemap(this);
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

        public void UpdateObjects(Player player)
        {
            Tilemap.Update(player);
        }

        private void GenerateLevel()
        {
            Tilemap.FillWithTile(Bounds, Tile.TileType.wall);
            ChamberTree = new ChamberTree(Bounds.Size, dungeon.MinRooms, dungeon.MaxRooms, dungeon.MinRoomWidth, dungeon.MaxRoomWidth,
                dungeon.MinRoomHeight, dungeon.MaxRoomHeight, dungeon.ForceRegularRooms);
            ChamberTree.FillChambersWithTile(Tile.TileType.floor, Tilemap);
            ChamberTree.FillPassagesWithTile(Tile.TileType.floor, Tilemap);

            Entrance = new Point(ChamberTree.StartingChamber.Center.X, ChamberTree.StartingChamber.Center.Y);

            if (Utilities.PassPercentileRoll(dungeon.CaveChance))
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
            Point pos = Tilemap.RandomPosition(ChamberTree.GetRandomChamber());

            if (CanPlaceCreature(pos))
            {
                dungeon.Generator.NewMonster(this, pos, LevelIndex);
                return true;
            }
            return false;
        }

        private bool TryPlaceItem()
        {
            Point pos = Tilemap.RandomPosition(ChamberTree.GetRandomChamber());

            if (CanPlaceItem(pos))
            {
                dungeon.Generator.NewWeapon(this, pos, LevelIndex);
                return true;
            }
            return false;
        }

        private bool TryPlacePlant()
        {
            Point pos = Tilemap.RandomPosition(ChamberTree.GetRandomChamber());

            if (CanPlaceCreature(pos))
            {
                new HealingPlant(this, pos);
                return true;
            }
            return false;
        }

        bool CanPlaceCreature(Point pos)
        {
            return Tilemap.ContainsPosition(pos) && Tilemap.IsWalkable(pos) &&
                Tilemap.Creatures.GetMappable(pos) == null && Tilemap.GetTile(pos).Type != Tile.TileType.exit;
        }

        bool CanPlaceItem(Point pos)
        {
            return Tilemap.ContainsPosition(pos) && Tilemap.IsWalkable(pos) &&
                (Tilemap.Creatures.GetMappable(pos) == null || Tilemap.Creatures.GetMappable(pos) is Monster) &&
                Tilemap.GetTile(pos).Type != Tile.TileType.exit;
        }

        public void GoDownOneLevel(Player player)
        {
            ILocation below = dungeon.NewLevel();

            player.Place(below, new Point(below.Entrance.X + 1, below.Entrance.Y));
            below.Tilemap.UpdateFogOfWar(player);
            below.Tilemap.UpdateFieldOfVisibility(player);
        }
    }
}
