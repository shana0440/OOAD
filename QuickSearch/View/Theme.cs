using QuickSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace QuickSearch.View
{
    public class Theme
    {
        public SolidColorBrush SearchbarColor;
        public SolidColorBrush SearchbarBorderColor;
        public SolidColorBrush ListBoxGroupBackgroundColor;
        public SolidColorBrush ForegroundColor;
        public BitmapImage LoadingImage;
        
        string _themeName;

        public Theme(string themeName)
        {
            _themeName = themeName;
            loadTheme(_themeName);
        }

        void loadTheme(string themeName)
        {
            var themePath = String.Format(@".\themes\{0}", themeName);
            var configPath = String.Format(@"{0}\config.xml", themePath);
            var doc = new XmlDocument();
            doc.Load(configPath);
            var elements = doc.DocumentElement;
            SearchbarColor = ConvertStringToSolidColorBrush(elements.SelectSingleNode("/Theme/BackgroundColor").InnerText);
            SearchbarBorderColor = ConvertStringToSolidColorBrush(elements.SelectSingleNode("/Theme/BorderColor").InnerText);
            ListBoxGroupBackgroundColor = ConvertStringToSolidColorBrush(elements.SelectSingleNode("/Theme/GroupBackgroundColor").InnerText);
            ForegroundColor = ConvertStringToSolidColorBrush(elements.SelectSingleNode("/Theme/ForegroundColor").InnerText);

            var loadingImagePath = String.Format(@"{0}\{1}", themePath, elements.SelectSingleNode("/Theme/LoadingImage")?.InnerText);
            LoadingImage = BitmapToBitmapImage.Transform((System.Drawing.Bitmap)System.Drawing.Image.FromFile(loadingImagePath));
        }

        public static SolidColorBrush ConvertStringToSolidColorBrush(string colorFormat)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorFormat));
        }
    }
}
