using System;
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
            string result = _translator.Translate();
            string expected = "<div class=\"compTitle mb-10\"><h3 class=\"title\"><span class=\"fz-s mb-10\">n.名詞</span></h3> </div><ul class=\"compArticleList mb-15 ml-10\"><li class=\"ov-a fstlst\"><h4>1. 蘋果[C]</h4><span style=\"margin-left: 16px; display:  block; color: #797979;\" id=\"example\" class=\"fc-2nd example dict\">An <b><b>apple</b></b> a day keeps the doctor away. <span style=\"margin-left: 0; display: inline; color: #797979; \" class=\"example_translation\">一日一蘋果，醫生不登門。</span></span></li></ul>";
            Assert.AreEqual(expected, result);
        }
    }
}
