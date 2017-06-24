using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.Dictionary
{
    class DictionaryResultItem : ResultItem
    {
        string _word;
        Definition _definition;

        public DictionaryResultItem(string word, Definition definition)
        {
            GroupName = "字典";
            Title = word;
            Priority = Config.DictionaryPriority;
            _icon = Properties.Resources.dictionary;
            _word = word;
            _definition = definition;
        }

        public override void GenerateContent(StackPanel contentView)
        {
            contentView.Children.Clear();

            var explanationTitle = new Label { Content = "解釋" };
            explanationTitle.FontSize = 18;
            explanationTitle.Margin = new Thickness(10, 10, 0, 0);
            explanationTitle.Foreground = Brushes.Gray;
            contentView.Children.Add(explanationTitle);
            foreach (var explanation in _definition.explanations)
            {
                var explanationLabel = new TextBlock { Text = explanation, TextWrapping = TextWrapping.WrapWithOverflow };
                explanationLabel.Margin = new Thickness(10, 0, 0, 0);
                explanationLabel.Foreground = ((SolidColorBrush)Application.Current.Resources["ForegroundColor"]);
                contentView.Children.Add(explanationLabel);
            }

            var exampleTitle = new Label { Content = "例句" };
            exampleTitle.FontSize = 18;
            exampleTitle.Margin = new Thickness(10, 10, 0, 0);
            exampleTitle.Foreground = Brushes.Gray;
            contentView.Children.Add(exampleTitle);
            foreach (var example in _definition.examples)
            {
                var originExample = new TextBlock { Text = example.origin, TextWrapping = TextWrapping.WrapWithOverflow };
                originExample.Margin = new Thickness(10, 0, 0, 5);
                originExample.Foreground = ((SolidColorBrush)Application.Current.Resources["ForegroundColor"]);
                contentView.Children.Add(originExample);

                var translatedExample = new TextBlock { Text = example.translated, TextWrapping = TextWrapping.WrapWithOverflow };
                translatedExample.Margin = new Thickness(10, 0, 0, 10);
                translatedExample.Foreground = Brushes.LightGray;
                contentView.Children.Add(translatedExample);
            }
        }

        public override void OpenResource()
        {
            var url = String.Format(Config.DirectoryUrl, _word);
            Process.Start(url);
        }
    }
}
