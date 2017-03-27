using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace QuickSearch.Models.ResultItemsFactory
{
    public interface IResultItem
    {
        int OriginIndex { get; set; }
        string GroupName { get; set; }
        string OriginGroupName { get; }
        int Priority { get; set; }
        string Title { get; set; }
        bool IsSelected { get; set; }
        BitmapImage Icon { get; }

        void OpenResource();
        void GenerateContent(StackPanel contentView);
    }
}
