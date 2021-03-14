using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class GraphicsTests
    {
        [TestMethod]
        public void Graphics_Initializes()
        {
            try
            {
                var graphics = new Graphics();
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
