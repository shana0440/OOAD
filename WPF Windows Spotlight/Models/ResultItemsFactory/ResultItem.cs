﻿using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPF_Windows_Spotlight.Models.ResultItemsFactory
{
    abstract class ResultItem : IResultItem
    {
        protected Bitmap _icon;
        public string GroupName { get; set; }
        public bool IsSelected { get; set; }
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

    }
}
