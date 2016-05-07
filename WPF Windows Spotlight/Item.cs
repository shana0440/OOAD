using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace WPF_Windows_Spotlight
{
    public class Item : INotifyPropertyChanged
    {
        private string _title;
        private string _content;
        private Bitmap _icon;

        public Item(string title)
        {
            _title = title;
        }

        public string Title
        {
            get { return _title; }
            set { 
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                NotifyPropertyChanged("Content");
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        internal void SetIcon(Bitmap bitmap)
        {
            _icon = bitmap;
        }
    }
}
