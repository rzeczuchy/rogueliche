using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class Dungeon
    {
        private int levelIndex;
        private int maxRoomHeight;
        private int minRoomHeight;
        private int maxRoomWidth;
        private int minRoomWidth;
        private int minWidth;
        private int minHeight;
        private int minRooms;
        private int maxRooms;

        public Dungeon()
        {
            Generator = new Generator();
            Width = 200;
            Height = 200;
            LevelIndex = 0;
            MinRooms = 6;
            MaxRooms = 26;
            MinRoomWidth = 3;
            MaxRoomWidth = 7;
            MinRoomHeight = 3;
            MaxRoomHeight = 7;
            MonstersPerRoom = 0.5;
            WeaponsPerRoom = 0.2;
            PlantsPerRoom = 0.3;
            CaveChance = 10;
            ForceRegularRooms = false;
        }

        public int LevelIndex { get => levelIndex; private set => levelIndex = Utilities.Clamp(value, 0, 99999); }
        public Generator Generator { get; private set; }
        public double MonstersPerRoom { get; set; }
        public double WeaponsPerRoom { get; set; }
        public double PlantsPerRoom { get; set; }
        public int Width { get => minWidth; set => minWidth = Utilities.Clamp(value, 20, 250); }
        public int Height { get => minHeight; set => minHeight = Utilities.Clamp(value, 20, 250); }
        public int MinRooms { get => minRooms; set => minRooms = Utilities.Clamp(value, 2, 250); }
        public int MaxRooms { get => maxRooms; set => maxRooms = Utilities.Clamp(value, 2, 250); }
        public int MinRoomWidth { get => minRoomWidth; set => minRoomWidth = Utilities.Clamp(value, 3, 10); }
        public int MaxRoomWidth { get => maxRoomWidth; set => maxRoomWidth = Utilities.Clamp(value, 3, 10); }
        public int MinRoomHeight { get => minRoomHeight; set => minRoomHeight = Utilities.Clamp(value, 3, 10); }
        public int MaxRoomHeight { get => maxRoomHeight; set => maxRoomHeight = Utilities.Clamp(value, 3, 10); }
        public int CaveChance { get; set; }
        public bool ForceRegularRooms { get; set; }
        
        public ILocation NewLevel()
        {
            LevelIndex++;
            return new DungeonLevel(this, LevelIndex);
        }
    }
}
