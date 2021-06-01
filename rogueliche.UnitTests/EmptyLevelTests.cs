using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class EmptyLevelTests
    {
        [TestMethod]
        public void EmptyLevel_Initializes()
        {
            try
            {
                var level = new EmptyLevel("test", 0, 0);
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
