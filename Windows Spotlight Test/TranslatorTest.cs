using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class TranslatorTest
    {
        private Translator _translator;
        [TestInitialize]
        public void TestInit()
        {
            _translator = new Translator();
        }

        [TestMethod]
        public void TestTranslate()
        {
            _translator.Word = "apple";
            var result = _translator.Translate();
            var defined = new KeyValuePair<string, string>("1. 蘋果[C]", "An apple a day keeps the doctor away. 一日一蘋果，醫生不登門。");
            var list = new List<KeyValuePair<string, string>> { defined };
            var expected = new List<KeyValuePair<string, List<KeyValuePair<string, string>>>> { new KeyValuePair<string, List<KeyValuePair<string, string>>>("n.名詞", list) };
            Assert.AreEqual(expected[0].Key, result[0].Key);
            Assert.AreEqual(expected[0].Value[0].Key, result[0].Value[0].Key);
            Assert.AreEqual(expected[0].Value[0].Value, result[0].Value[0].Value);
        }

        [TestMethod]
        public void TestTranslateError()
        {
            _translator.Word = "asdjklqhweklzsc";
            var result = _translator.Translate();
            string expected = "Not Found";
            Assert.AreEqual(expected, result);
        }
    }
}
