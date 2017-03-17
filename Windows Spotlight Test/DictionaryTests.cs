using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickSearch.Models.Dictionary;
using System.Collections.Generic;
using System.Net;
using Telerik.JustMock;
using System.Net.NetworkInformation;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class DictionaryTests
    {
        [TestMethod]
        [ExpectedException(typeof(WebException), "沒有連接至網際網路")]
        public void TestNetworkAvailable()
        {
            Mock.SetupStatic(typeof(NetworkInterface), Behavior.Strict, StaticConstructor.Mocked);
            Mock.Arrange(() => NetworkInterface.GetIsNetworkAvailable()).Returns(false);
            Dictionary dictionary = new Dictionary();
            Definition results = dictionary.Search("apple");
            Assert.Fail();
        }


        [TestMethod]
        public void TestSearch()
        {
            Dictionary dictionary = new Dictionary();
            Definition definition = dictionary.Search("style");
            Assert.AreEqual("n.方式；樣式；時髦；儀錶，品位；", definition.explanations[0]);
            Assert.AreEqual("The word style was used in our definition of fashion.", definition.examples[0].origin);
            Assert.AreEqual("款式(style)這個詞曾用來給時裝下定義.", definition.examples[0].translated);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "沒有找到符合的結果")]
        public void TestSearchNoResult()
        {
            Dictionary dictionary = new Dictionary();
            Definition results = dictionary.Search("abcasd");
            Assert.Fail();
        }
    }
}
