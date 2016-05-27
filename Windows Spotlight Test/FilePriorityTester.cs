using System;
using System.IO;
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
            var fileInfo = new FileInfo(@"C:\Users\stones\Documents\Visual Studio 2013\Projects\WPF Windows Spotlight\Windows Spotlight Test\AdapterTest.cs");
            var file = new FolderOrFile(fileInfo);
            var count = filePriority.PriorityUp(file);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void TestInPriorityFile()
        {
            var filePriority = new FilePriority();
            Assert.IsTrue(filePriority.InPriorityFile("Adapter"));
        }

    }
}
