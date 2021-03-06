﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.CurrencyConverter
{
    class CurrencyConverterResultItem : ResultItem
    {
        string _origin;

        public CurrencyConverterResultItem(string origin, string afterConverted)
        {
            GroupName = "匯率換算";
            Priority = Config.CurrencyConverterPriority;
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

            expression.Foreground = Brushes.LightGray;
            expression.HorizontalContentAlignment = HorizontalAlignment.Center;
            Thickness expMargin = new Thickness(0, 100, 0, 10);
            expression.Margin = expMargin;
            contentView.Children.Add(expression);

            Border hr = new Border();
            Color hrColor = ((SolidColorBrush)Application.Current.Resources["SearchbarBorderColor"]).Color;
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
            answer.Foreground = ((SolidColorBrush)Application.Current.Resources["ForegroundColor"]);
            contentView.Children.Add(answer);
            
            Label copyright = new Label();
            copyright.Content = "資料來源 GOOGLE";
            copyright.FontSize = 12;
            copyright.HorizontalContentAlignment = HorizontalAlignment.Center;
            Thickness copyrightMargin = new Thickness(0, 100, 0, 0);
            copyright.Margin = copyrightMargin;
            copyright.Foreground = Brushes.Gray;
            contentView.Children.Add(copyright);
        }

        public override void OpenResource()
        {
        }
    }
}
