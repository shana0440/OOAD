using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Windows_Spotlight.Foundation.ItemType
{
    public class TranslateItem : Item
    {
        private readonly string _url;
        private readonly List<KeyValuePair<string, List<KeyValuePair<string, string>>>> _sections;

        public TranslateItem(string title, string url, List<KeyValuePair<string, List<KeyValuePair<string, string>>>> sections, string name = "", int weight = 0) : base(title, name, weight)
        {
            _url = url;
            _sections = sections;
        }

        public string Url
        {
            get { return _url; }
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
            foreach (var section in _sections)
            {
                var part = new Label {Content = section.Key};
                contentView.Children.Add(part);
                foreach (var pair in section.Value)
                {
                    var define = new Label { Content = pair.Key };
                    define.Margin = new Thickness(10, 0, 0, 0);
                    contentView.Children.Add(define);
                    if (pair.Value != "")
                    {
                        var example = new Label { Content = pair.Value };
                        example.Margin = new Thickness(10, 0, 0, 0);
                        contentView.Children.Add(example);
                    }
                }
            }
        }

    }
}
