using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class FilePriorityTester
    {
        [TestMethod]
        public void TestPriorityUp()
        {
            var filePriority = new FilePriority();
            var count = filePriority.PriorityUp("Notepad.lnk");
            Assert.AreEqual(3, count);
        }

    }
}
