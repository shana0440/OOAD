using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF_Windows_Spotlight.Foundation;

namespace WPF_Windows_Spotlight.Foundation.ItemType
{
    public class TranslateItem : Item
    {
        private readonly string _url;
        private readonly string _html;

        public TranslateItem(string title, string url, string html) : base(title)
        {
            _html = html;
            _url = url;
        }

        public string Url
        {
            get { return _url; }
        }

        public string Html
        {
            get { return _html; }
        }

        public override void Open()
        {
            try
            {
                if (_url != null)
                {
                    System.Diagnostics.Process.Start(_url);
                }
            }
            catch (Win32Exception e)
            {
                throw new Exception("Can't open this web site");
            }   
        }

        public override void GenerateContent(StackPanel contentView)
        {
            contentView.Children.Clear();
            WebBrowser browser = new WebBrowser();
            browser.Width = contentView.Width - 5;
            browser.Height = contentView.Height - 15;
            browser.NavigateToString(Html);
            browser.Margin = new Thickness(5, 10, 0, 5);
            contentView.Children.Add(browser);
        }

    }
}
