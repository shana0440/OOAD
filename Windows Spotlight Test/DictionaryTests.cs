using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Models.Dictionary;
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
            List<ExplanationSection> results = dictionary.Search("apple");
            Assert.Fail();
        }


        [TestMethod]
        public void TestSearch()
        {
            Dictionary dictionary = new Dictionary();
            List<ExplanationSection> results = dictionary.Search("apple");
            Assert.AreEqual("n.名詞", results[0].PartOfSpeech);
            Assert.AreEqual("1. 蘋果[C]", results[0].Interpretations[0].Interpretation);
            Assert.AreEqual("An apple a day keeps the doctor away. 一日一蘋果，醫生不登門。", results[0].Interpretations[0].Example);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "沒有找到符合的結果")]
        public void TestSearchNoResult()
        {
            Dictionary dictionary = new Dictionary();
            List<ExplanationSection> results = dictionary.Search("abcasd");
            Assert.Fail();
        }
    }
}
