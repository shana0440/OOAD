using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF_Windows_Spotlight.Models.ResultItemsFactory
{
    abstract class ResultItem : IResultItem, INotifyPropertyChanged
    {
        protected Bitmap _icon;
        private bool _isSelected;
        public string GroupName { get; set; }
        public bool IsSelected {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }
        public string Title { get; set; }
        public int Priority { get; set; }

        public abstract void GenerateContent(StackPanel contentView);
        public abstract void OpenResource();

        public BitmapImage Icon
        {
            get
            {
                if (_icon == null)
                {
                    throw new NullReferenceException();
                }
                return BitmapToBitmapImage.Transform(_icon);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


    }
}
