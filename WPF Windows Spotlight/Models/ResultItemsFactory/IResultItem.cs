using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPF_Windows_Spotlight.Models.ResultItemsFactory
{
    public interface IResultItem
    {
        string GroupName { get; set; }
        int Weight { get; set; }
        string Title { get; set; }
        bool IsSelected { get; set; }
        BitmapImage Icon { get; }

        void OpenResource();
        void GenerateContent();
    }
}
