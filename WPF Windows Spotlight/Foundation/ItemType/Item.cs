using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using WPF_Windows_Spotlight;
using WPF_Windows_Spotlight.Foundation;

namespace WPF_Windows_Spotlight.Foundation.ItemType
{
    public abstract class Item : INotifyPropertyChanged
    {
        private readonly string _title;
        private Bitmap _icon;
        private bool _isSelected;
        public string GroupName { get; set; }

        protected Item(string title, string groupName)
        {
            _title = title;
            GroupName = groupName;
        }

        public string Title
        {
            get { return _title; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set 
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        public BitmapImage Icon
        {
            get
            {
                if (_icon == null) return null;
                using (MemoryStream memory = new MemoryStream())
                {
                    try
                    {
                        _icon.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                        memory.Position = 0;
                        BitmapImage bitmapimage = new BitmapImage();
                        bitmapimage.BeginInit();
                        bitmapimage.StreamSource = memory;
                        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapimage.EndInit();

                        return bitmapimage;
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        return null;
                    }
                }
            }
        }

        public abstract void Open();
        public abstract void GenerateContent(StackPanel contentView);

        public void SetIcon(Bitmap bitmap)
        {
            _icon = bitmap;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
