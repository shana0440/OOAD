using QuickSearch.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace QuickSearch
{
    public class Config
    {
        public const string DirectoryUrl = "https://tw.voicetube.com/definition/{0}";
        public const string CurrencyConvertUrl = "https://www.google.com/finance/converter?a={0}&from={1}&to=TWD";
        public const string SearchEngineUrl = "https://www.google.com.tw/search?q={0}";

        public HashSet<Keys> KeyForHide;
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
            KeyForHide = new HashSet<Keys> { Keys.Escape };
            KeyForOpenAndHide = new HashSet<Keys> { Keys.LWin, Keys.Space };
            SetTheme("Dark");
        }

        public void SetHotKey(HashSet<Keys> hotkey)
        {
            KeyForOpenAndHide = hotkey;
            // save to setting xml
        }

        public void SetTheme(string theme)
        {
            this.Theme = new Theme(theme);
            ApplyTheme(this.Theme);
            // save to setting xml
        }

        void ApplyTheme(Theme theme)
        {
            var fields = theme.GetType().GetFields();
            foreach (var field in fields)
            {
                var test = System.Windows.Application.Current.Resources[field.Name] = theme.GetType().GetField(field.Name).GetValue(theme);
            }
        }
    }
}
