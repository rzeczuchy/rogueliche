using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Dungeon
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
            Width = 100;
            Height = 100;
            LevelIndex = 0;
            MinRooms = 10;
            MaxRooms = 30;
            MinRoomWidth = 3;
            MaxRoomWidth = 7;
            MinRoomHeight = 3;
            MaxRoomHeight = 7;
            MonstersPerLevel = 10;
            WeaponsPerLevel = 3;
            CaveChance = 50;
            ForceRegularRooms = false;
        }

        public int LevelIndex { get => levelIndex; private set => levelIndex = Numbers.Clamp(value, 0, 99999); }
        public Generator Generator { get; private set; }
        public int MonstersPerLevel { get; set; }
        public int WeaponsPerLevel { get; set; }
        public int Width { get => minWidth; set => minWidth = Numbers.Clamp(value, 20, 250); }
        public int Height { get => minHeight; set => minHeight = Numbers.Clamp(value, 20, 250); }
        public int MinRooms { get => minRooms; set => minRooms = Numbers.Clamp(value, 2, 250); }
        public int MaxRooms { get => maxRooms; set => maxRooms = Numbers.Clamp(value, 2, 250); }
        public int MinRoomWidth { get => minRoomWidth; set => minRoomWidth = Numbers.Clamp(value, 3, 10); }
        public int MaxRoomWidth { get => maxRoomWidth; set => maxRoomWidth = Numbers.Clamp(value, 3, 10); }
        public int MinRoomHeight { get => minRoomHeight; set => minRoomHeight = Numbers.Clamp(value, 3, 10); }
        public int MaxRoomHeight { get => maxRoomHeight; set => maxRoomHeight = Numbers.Clamp(value, 3, 10); }
        public int CaveChance { get; set; }
        public bool ForceRegularRooms { get; set; }


        public DungeonLevel NewLevel()
        {
            LevelIndex++;
            return new DungeonLevel(this, LevelIndex);
        }

    }
}
