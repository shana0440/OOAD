using QuickSearch.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;

namespace QuickSearch
{
    public class Config
    {
        public const string DirectoryUrl = "https://tw.voicetube.com/definition/{0}";
        public const string CurrencyConvertUrl = "https://www.google.com/finance/converter?a={0}&from={1}&to=TWD";
        public const string SearchEngineUrl = "https://www.google.com.tw/search?q={0}";

        public HashSet<Keys> KeyForHide = new HashSet<Keys> { Keys.Escape };
        public HashSet<Keys> KeyForOpenAndHide;

        public const int SearchbarWidth = 700;
        public const int SearchbarHeight = 420;
        public const int InputHieght = 50;

        public const int CalculatorPriority = 999;
        public const int CurrencyConverterPriority = 999;
        public const int DictionaryPriority = 200;
        public const int FileSystemPriority = 100;
        public const int SearchEnginePriority = 50;

        public Theme Theme;

        public Config()
        {
            // read setting xml
            LoadSetting();
        }

        public void SetHotKey(HashSet<Keys> hotkey)
        {
            KeyForOpenAndHide = hotkey;
            // save to setting xml
            SaveSettingProperty("HotKey", String.Join("+", hotkey));
        }

        public void SetTheme(string theme)
        {
            this.Theme = new Theme(theme);
            ApplyTheme(this.Theme);
            // save to setting xml
            SaveSettingProperty("Theme", theme);
        }

        void ApplyTheme(Theme theme)
        {
            var fields = theme.GetType().GetFields();
            foreach (var field in fields)
            {
                var test = System.Windows.Application.Current.Resources[field.Name] = theme.GetType().GetField(field.Name).GetValue(theme);
            }
        }

        void LoadSetting()
        {
            var path = @".\setting.xml";
            var doc = new XmlDocument();
            doc.Load(path);
            var eles = doc.DocumentElement;
            var hotKeyString = eles.SelectSingleNode("/Config/HotKey").InnerText;
            var hotKeySplit = hotKeyString.Split('+');
            HashSet<Keys> hotKey = new HashSet<Keys>();
            foreach (var key in hotKeySplit)
            {
                var k = (Keys)Enum.Parse(typeof(Keys), key, true);
                hotKey.Add(k);
            }
            KeyForOpenAndHide = hotKey;
            var theme = eles.SelectSingleNode("/Config/Theme").InnerText;
            this.Theme = new Theme(theme);
            ApplyTheme(this.Theme);
        }

        void SaveSettingProperty(string name, string value)
        {
            var path = @".\setting.xml";
            var doc = new XmlDocument();
            doc.Load(path);
            var eles = doc.DocumentElement;
            var selector = String.Format("/Config/{0}", name);
            var node = eles.SelectSingleNode(selector);
            node.InnerText = value;
            doc.Save(path);
        }
    }
}
