using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;
using System.IO;

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
            _fileSystem.Keyword = "putty.exe";
            var reslut = _fileSystem.Search();
        }
    }
}
