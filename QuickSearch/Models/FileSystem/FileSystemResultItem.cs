using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QuickSearch.Models.ResultItemsFactory;
using System.Xml;

namespace QuickSearch.Models.FileSystem
{
    abstract class FileSystemResultItem : ResultItem
    {
        string _filePath;
        protected Dictionary<string, string> _property = new Dictionary<string, string>();

        public FileSystemResultItem(string filePath)
        {
            GroupName = "檔案或資料夾";
            Priority = Config.FileSystemPriority;
            _filePath = filePath;
            _property.Add("Created", "");
            _property.Add("Modified", "");
            _property.Add("Accessed", "");
        }

        protected void SetInfo(FileSystemInfo info)
        {
            Title = info.Name;
            _property["Created"] = info.CreationTime.ToString();
            _property["Modified"] = info.LastWriteTime.ToString();
            _property["Accessed"] = info.LastAccessTime.ToString();
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

            var title = new TextBlock();
            title.Text = Title;
            title.FontSize = 24;
            title.TextTrimming = TextTrimming.WordEllipsis;
            title.TextAlignment = TextAlignment.Center;
            title.Foreground = ((SolidColorBrush)Application.Current.Resources["ForegroundColor"]);
            contentView.Children.Add(title);

            var hr = new Border();
            var hrColor = ((SolidColorBrush)Application.Current.Resources["SearchbarBorderColor"]).Color;
            hr.Height = 1;
            hr.BorderBrush = new SolidColorBrush(hrColor);
            hr.BorderThickness = new Thickness(0, 1, 0, 0);
            hr.Width = contentView.Width - 30;
            hr.Margin = new Thickness(15, 40, 15, 15);
            contentView.Children.Add(hr);

            foreach (var property in _property)
            {
                var wrap = new WrapPanel();
                var name = new Label();
                name.Content = property.Key;
                var value = new Label();
                value.Content = property.Value;
                name.Width = value.Width = contentView.Width / 2 - 35;
                name.HorizontalContentAlignment = HorizontalAlignment.Right;
                value.HorizontalContentAlignment = HorizontalAlignment.Left;
                name.Foreground = Brushes.LightGray;
                value.Foreground = ((SolidColorBrush)Application.Current.Resources["SearchbarBorderColor"]);

                wrap.Children.Add(name);
                wrap.Children.Add(value);
                contentView.Children.Add(wrap);
            }
        }

        public override void OpenResource()
        {
            Record(_filePath);
            Process.Start(_filePath);
        }

        void Record(string filePath)
        {
            var historyRecordsFile = @"fileOpenHistory.xml";
            var xml = new XmlDocument();
            if (!File.Exists(historyRecordsFile))
            {
                var recordsElem = xml.CreateElement("Records");
                xml.AppendChild(recordsElem);
                AddNewRecord(xml);
            }
            else
            {
                xml.Load(historyRecordsFile);
                var node = xml.SelectSingleNode(String.Format("//Records/Record[@Title='{0}']", Title));
                if (node == null)
                    AddNewRecord(xml);
                else
                    IncreaseCount(node);
            }
            xml.Save(historyRecordsFile);
        }

        void AddNewRecord(XmlDocument xml)
        {
            var recordElem = xml.CreateElement("Record");
            recordElem.SetAttribute("Title", Title);
            recordElem.SetAttribute("Path", _filePath);
            recordElem.SetAttribute("OpenCount", "1");

            xml.SelectSingleNode("//Records").AppendChild(recordElem);
        }

        private void IncreaseCount(XmlNode node)
        {
            var count = Int32.Parse(node.Attributes["OpenCount"].Value);
            count++;
            node.Attributes["OpenCount"].Value = count.ToString();
        }
    }
}
