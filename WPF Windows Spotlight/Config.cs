using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace QuickSearch
{
    class Config
    {
        public const string DirectoryUrl = "https://tw.voicetube.com/definition/{0}";
        public const string CurrencyConvertUrl = "https://www.google.com/finance/converter?a={0}&from={1}&to=TWD";
        public const string SearchEngineUrl = "https://www.google.com.tw/search?q={0}";
        
        public static HashSet<Keys> KeyForHide = new HashSet<Keys> { Keys.Escape };
        public static HashSet<Keys> KeyForOpenAndHide = new HashSet<Keys> { Keys.Control, Keys.Space };

        public const int SearchbarWidth = 700;
        public const int SearchbarHeight = 420;
        public const int InputHieght = 50;

        public const string SearchbarColor = "#FF252525";
        public const string SearchbarBorderColor = "#FF3a3a3a";

    }
}
