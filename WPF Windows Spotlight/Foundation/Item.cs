using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using WPF_Windows_Spotlight;

namespace WPF_Windows_Spotlight.Foundation
{
    public class Item
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
            set { _title = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
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

        public void Open()
        {
            if (_content != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(_content);
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine("無法開啟");
                }
            }            
        }

        public void SetIcon(Bitmap bitmap)
        {
            _icon = bitmap;
        }
    }
}
