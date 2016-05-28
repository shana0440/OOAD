using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class SearchEngineTester
    {
        private SearchEngine _system;

        [TestInitialize]
        public void Init()
        {
            _system = new SearchEngine();
        }

        [TestMethod]
        public void TestSearch()
        {
            var expected = new List<WebSite>();
            expected.Add(new WebSite("How people build software · GitHub", "Online project hosting using Git. Includes source-code browser, in-line editing, wikis, and ticketing. Free for public open-source code. Commercial closed source ...", "https://github.com/"));
            _system.SetKeyword("github");
            var results = _system.Search();
            for (int i = 0; i < expected.Count; i++)
            {
                var result = (WebSite) results[i];
                Assert.AreEqual(expected[i].Title, result.Title);
                Assert.AreEqual(expected[i].Intro, result.Intro);
                Assert.AreEqual(expected[i].Url, result.Url);
            }
        }

        [TestMethod]
        public void TestSearchNoResult()
        {
            _system.SetKeyword("qowiejalskdfjqlwkehdskljfhqaksjdslkd");
            var results = _system.Search();
            Assert.AreEqual(0, results.Count);
        }
    }
}
