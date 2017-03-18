using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickSearch.Models.FileSystem;
using System.Collections.Generic;
using Telerik.JustMock;

namespace QuickSearchTest
{
    [TestClass]
    public class FileSystemTests
    {
        [TestMethod]
        public void TestSearch()
        {
            var fileSystem = new FileSystem();
            List<string> fakeResults = new List<string>() { "object1" };
            Mock.SetupStatic(typeof(Everything), Behavior.Strict, StaticConstructor.Mocked);
            Mock.Arrange(() => Everything.Search("keyword", 30)).Returns(fakeResults);

            List<string> results = fileSystem.Search("keyword");

            Assert.AreEqual(1, results.Count);
        }
    }
}
