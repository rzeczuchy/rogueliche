using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class DungeonTests
    {
        [TestMethod]
        public void NewLevel_ReturnsLocation()
        {
            var dungeon = new Dungeon();
            var level = dungeon.NewLevel();

            Assert.IsInstanceOfType(level, typeof(ILocation));
        }

        [TestMethod]
        public void NewLevel_ChangesIndex()
        {
            var dungeon = new Dungeon();
            var level1 = dungeon.NewLevel() as DungeonLevel;
            var level2 = dungeon.NewLevel() as DungeonLevel;

            Assert.AreEqual(level1.LevelIndex, 1);
            Assert.AreEqual(level2.LevelIndex, 2);
        }
    }
}
