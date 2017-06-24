using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuickSearch.Models.ResultItemsFactory
{
    abstract class ResultItem : IResultItem, INotifyPropertyChanged
    {
        protected Bitmap _icon;
        private bool _isSelected;
        private string _originGroupName;
        private string _groupName;
        private int _originIndex = -1;

        public int OriginIndex {
            get
            {
                return _originIndex;
            }
            set
            {
                _originIndex = (_originIndex == -1) ? value : _originIndex;
            }
        }
        public string OriginGroupName {
            get
            {
                return _originGroupName;
            }
        }

        public string GroupName {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
                _originGroupName = _originGroupName ?? value;
            }
        }

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
