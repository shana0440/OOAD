using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_Windows_Spotlight.Foundation;

namespace WPF_Windows_Spotlight.Foundation.ItemType
{
    public class FileItem : Item
    {
        private readonly FolderOrFile _folderOrFile;

        public FileItem(FolderOrFile folderOrFile, string name = "")
            : base(folderOrFile.Name, name)
        {
            _folderOrFile = folderOrFile;
        }

        public string FullName
        {
            get { return _folderOrFile.FullName; }
        }

        public List<KeyValuePair<string, string>> GetProperty()
        {
            var propertys = new List<KeyValuePair<string, string>>();
            propertys.Add(new KeyValuePair<string, string>("Created", _folderOrFile.CreationDate));
            propertys.Add(new KeyValuePair<string, string>("Modified", _folderOrFile.LastWriteDate));
            propertys.Add(new KeyValuePair<string, string>("Accessed", _folderOrFile.LastAccessDate));
            return propertys;
        }

        public override void Open()
        {
            try
            {
                if (_folderOrFile != null)
                {
                    System.Diagnostics.Process.Start(_folderOrFile.FullName);
                    var filePriority = new FilePriority();
                    filePriority.PriorityUp(_folderOrFile);
                }
            }
            catch (Win32Exception e)
            {
                throw new Exception("Can't open this file or folder");
            }
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

            var hr = new Border();
            var hrColor = (Color)ColorConverter.ConvertFromString("#FFD7D7D7");
            hr.Height = 1;
            hr.BorderBrush = new SolidColorBrush(hrColor);
            hr.BorderThickness = new Thickness(0, 1, 0, 0);
            hr.Width = contentView.Width - 30;
            hr.Margin = new Thickness(15, 40, 15, 15);
            contentView.Children.Add(hr);

            var propertys = GetProperty();
            foreach (var property in propertys)
            {
                var wrap = new WrapPanel();
                var name = new Label();
                name.Content = property.Key;
                var value = new Label();
                value.Content = property.Value;
                name.Width = value.Width = contentView.Width / 2;
                name.HorizontalContentAlignment = HorizontalAlignment.Right;
                value.HorizontalContentAlignment = HorizontalAlignment.Left;
                name.Foreground = Brushes.Gray;

                wrap.Children.Add(name);
                wrap.Children.Add(value);
                contentView.Children.Add(wrap);
            }
        }
    }
}
