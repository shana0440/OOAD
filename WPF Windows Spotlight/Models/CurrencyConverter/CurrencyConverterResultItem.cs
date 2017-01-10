using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.CurrencyConverter
{
    class CurrencyConverterResultItem : ResultItem
    {
        string _origin;

        public CurrencyConverterResultItem(string origin, string afterConverted)
        {
            GroupName = "匯率換算";
            Priority = 999;
            Title = afterConverted;
            _icon = Properties.Resources.exchange;
            _origin = origin;
        }

        public override void GenerateContent(StackPanel contentView)
        {
            contentView.Children.Clear();
            Label expression = new Label();
            expression.Content = _origin.ToUpper() + "=";
            expression.FontSize = 24;
            Color fontColor = (Color)ColorConverter.ConvertFromString("#434343");
            expression.BorderBrush = new SolidColorBrush(fontColor);

            expression.Foreground = expression.BorderBrush;
            expression.HorizontalContentAlignment = HorizontalAlignment.Center;
            Thickness expMargin = new Thickness(0, 100, 0, 10);
            expression.Margin = expMargin;
            contentView.Children.Add(expression);

            Border hr = new Border();
            Color hrColor = (Color)ColorConverter.ConvertFromString("#FFD7D7D7");
            hr.Height = 1;
            hr.BorderBrush = new SolidColorBrush(hrColor);
            hr.BorderThickness = new Thickness(0, 1, 0, 0);
            hr.Width = contentView.Width - 30;
            hr.Margin = new Thickness(15, 10, 15, 10);
            contentView.Children.Add(hr);

            Label answer = new Label();
            answer.Content = Title;
            answer.FontSize = 24;
            answer.HorizontalContentAlignment = HorizontalAlignment.Center;
            contentView.Children.Add(answer);
            
            Label copyright = new Label();
            copyright.Content = "資料來源 GOOGLE";
            copyright.FontSize = 12;
            copyright.HorizontalContentAlignment = HorizontalAlignment.Center;
            Color copyrightFontColor = (Color)ColorConverter.ConvertFromString("#888888");
            Thickness copyrightMargin = new Thickness(0, 100, 0, 0);
            copyright.Margin = copyrightMargin;
            copyright.Foreground = new SolidColorBrush(copyrightFontColor);
            contentView.Children.Add(copyright);
        }

        public override void OpenResource()
        {
        }
    }
}
