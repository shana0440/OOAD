using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Models.FileSystem;
using System.Collections.Generic;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class FileSystemTests
    {
        [TestMethod]
        public void TestSearchFile()
        {
            var fileSystem = new FileSystem();
            List<string> results = fileSystem.SearchFileOrFolder("FileSystemTests.cs");
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestSearchFolder()
        {
            var fileSystem = new FileSystem();
            List<string> results = fileSystem.SearchFileOrFolder("TestFileSystemFiles2");
            // 除了資料夾以外還有一個捷徑
            Assert.AreEqual(2, results.Count);
        }
    }
}
