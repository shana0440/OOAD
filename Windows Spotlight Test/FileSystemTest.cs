using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;
using System.IO;
using System.Collections.Generic;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class FileSystemTest
    {
        FileSystem _fileSystem;

        [TestInitialize]
        public void TestInit()
        {
            _fileSystem = new FileSystem();
        }

        [TestMethod]
        public void TestSearchFile()
        {
            Assert.AreEqual(1, _fileSystem.Search("putty.exe").Count);
        }
    }
}
