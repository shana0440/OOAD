using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.SearchEngine
{
    class SearchEngineResultItem : ResultItem
    {
        string _url;
        string _intro;

        public SearchEngineResultItem(string title, string url, string intro)
        {
            GroupName = "網頁";
            Title = title;
            Priority = 300;
            _icon = Properties.Resources.web;
            _url = url;
            _intro = intro;
        }

        public override void GenerateContent(StackPanel contentView)
        {
            contentView.Children.Clear();
            var img = new Image();
            img.Source = Icon;
            var imgMargin = new Thickness(125, 50, 125, 10);
            img.Margin = imgMargin;
            img.Width = 100;
            contentView.Children.Add(img);

            var title = new Label();
            title.Content = Title;
            title.FontSize = 24;
            title.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentView.Children.Add(title);

            var url = new Label();
            url.Content = _url;
            url.FontSize = 18;
            url.HorizontalContentAlignment = HorizontalAlignment.Center;
            url.Foreground = Brushes.Gray;
            contentView.Children.Add(url);

            var hr = new Border();
            var hrColor = (Color)ColorConverter.ConvertFromString("#FFD7D7D7");
            hr.Height = 1;
            hr.BorderBrush = new SolidColorBrush(hrColor);
            hr.BorderThickness = new Thickness(0, 1, 0, 0);
            hr.Width = contentView.Width - 30;
            hr.Margin = new Thickness(15, 20, 15, 15);
            contentView.Children.Add(hr);

            var wrap = new WrapPanel();
            var intro = new TextBlock();
            intro.TextWrapping = TextWrapping.WrapWithOverflow;
            intro.Width = contentView.Width;
            intro.Text = _intro;
            intro.HorizontalAlignment = HorizontalAlignment.Center;
            intro.Padding = new Thickness(20, 10, 20, 10);
            intro.Foreground = Brushes.Gray;

            wrap.Children.Add(intro);
            contentView.Children.Add(wrap);
        }

        public override void OpenResource()
        {
            Process.Start(_url);
        }
    }
}
