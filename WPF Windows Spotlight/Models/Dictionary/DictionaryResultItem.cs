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

namespace WPF_Windows_Spotlight.Models.Dictionary
{
    class DictionaryResultItem : ResultItem
    {
        string _word;
        List<ExplanationSection> _sections;

        public DictionaryResultItem(string word, List<ExplanationSection> sections)
        {
            GroupName = "字典";
            Title = word;
            Priority = 800;
            _icon = Properties.Resources.dictionary;
            _word = word;
            _sections = sections;
        }

        public override void GenerateContent(StackPanel contentView)
        {
            contentView.Children.Clear();
            foreach (var section in _sections)
            {
                var part = new Label { Content = section.PartOfSpeech };
                part.Foreground = Brushes.LightGray;
                contentView.Children.Add(part);
                foreach (var interpretation in section.Interpretations)
                {
                    var define = new Label { Content = interpretation.Interpretation };
                    define.Margin = new Thickness(10, 0, 0, 0);
                    define.Foreground = Brushes.White;
                    contentView.Children.Add(define);
                    if (interpretation.Example != "")
                    {
                        var example = new Label { Content = interpretation.Example };
                        example.Margin = new Thickness(10, 0, 0, 0);
                        example.Foreground = Brushes.LightGray;
                        contentView.Children.Add(example);
                    }
                }
            }
        }

        public override void OpenResource()
        {
            var url = String.Format(Config.DirectoryUrl, _word);
            Process.Start(url);
        }
    }
}
