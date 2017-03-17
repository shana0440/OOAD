using System.IO;

namespace QuickSearch.Models.FileSystem
{
    class FileSystemResultFolderItem : FileSystemResultItem
    {
        public FileSystemResultFolderItem(string filePath) : base(filePath)
        {
            _icon = Properties.Resources.folder_icon;

            var info = new DirectoryInfo(filePath);
            SetInfo(info);
        }

    }
}
