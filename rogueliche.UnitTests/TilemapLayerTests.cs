using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class TilemapLayerTests
    {
        [TestMethod]
        public void TilemapLayer_Initializes()
        {
            try
            {
                var dungeon = new Dungeon();
                var level = dungeon.NewLevel();
                var tilemap = new Tilemap(level, 100, 100);
                new TilemapLayer(tilemap);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Constructor_ThrowsExceptionWhenTilemapNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => { new TilemapLayer(null); });
        }
    }
}
