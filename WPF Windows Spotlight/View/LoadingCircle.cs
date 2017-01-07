using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPF_Windows_Spotlight
{
    public class LoadingCircle : INotifyPropertyChanged
    {
        private int _angle;
        private DispatcherTimer _incrementAngleTimer = new DispatcherTimer();
        private bool _searching = false;

        public int Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
                NotifyPropertyChanged("Angle");
            }
        }

        public void Start()
        {
            if (!_searching)
            {
                _incrementAngleTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                _incrementAngleTimer.Tick += IncrementAngle;
                _incrementAngleTimer.Start();
                _searching = true;
            }
        }

        private void IncrementAngle(object sender, EventArgs e)
        {
            Angle += 5;
        }

        public void Stop()
        {
            Angle = 0;
            _incrementAngleTimer.Stop();
            _incrementAngleTimer.Tick -= IncrementAngle;
            _searching = false;
        }

        public BitmapImage SearchImage
        {
            get
            {
                Bitmap searching = (Bitmap)Properties.Resources.loading;

                using (MemoryStream memory = new MemoryStream())
                {
                    try
                    {
                        searching.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
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

    }

}
