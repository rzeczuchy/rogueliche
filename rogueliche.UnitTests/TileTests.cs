using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class TileTests
    {
        [TestMethod]
        public void Tile_Initializes()
        {
            try
            {
                var tile = new Tile(Tile.TileType.wall);
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
