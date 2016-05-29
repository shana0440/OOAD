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
            string body =
                "<div class=\"compTitle mb-10\"><h3 class=\"title\"><span class=\"fz-s mb-10\">n.名詞</span></h3> </div><ul class=\"compArticleList mb-15 ml-10\"><li class=\"ov-a fstlst\"><h4>1. 蘋果[C]</h4><span style=\"margin-left: 16px; display:  block; color: #797979;\" id=\"example\" class=\"fc-2nd example dict\">An <b><b>apple</b></b> a day keeps the doctor away. <span style=\"margin-left: 0; display: inline; color: #797979; \" class=\"example_translation\">一日一蘋果，醫生不登門。</span></span></li></ul>";
            string head = "<link rel=\"stylesheet\" type=\"text/css\" href=\"https://s.yimg.com/zz/combo?kx/yucs/uh3/uh/1138/css/uh_non_mail-min.css&kx/yucs/uh3s/atomic/84/css/atomic-min.css&kx/yucs/uh_common/meta/3/css/meta-min.css&kx/yucs/uh3/top-bar/366/css/no_icons-min.css&kx/yucs/uh3/uh/1132/css/uh_ssl-min.css&pv/static/lib/srp-header-css-light_ee1bace10b4c01c090c2ee71401ddbc2.css&pv/static/lib/tw_srp-core-css-light-tw_65c5c5c9fbb14ab227f7e541d64e23dc.css\" class=\"inline\">";
            string expected = Translator.GenerateHtml(body, head);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestTranslateError()
        {
            _translator.Word = "asdjklqhweklzsc";
            string result = _translator.Translate();
            string expected = "Not Found";
            Assert.AreEqual(expected, result);
        }
    }
}
