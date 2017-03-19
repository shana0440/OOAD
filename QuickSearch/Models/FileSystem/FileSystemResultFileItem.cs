using System;
using System.IO;

namespace QuickSearch.Models.FileSystem
{
    class FileSystemResultFileItem : FileSystemResultItem
    {
        public FileSystemResultFileItem(string filePath) : base(filePath)
        {
            var ico = IconReader.GetFileIcon(filePath, IconReader.IconSize.Large);
            var bmp = ico.ToBitmap();
            bmp.MakeTransparent();
            _icon = bmp;
            var info = new FileInfo(filePath);
            SetInfo(info);
        }
        
    }
}
