using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QuickSearch.Models
{
    class BitmapToBitmapImage
    {
        static public BitmapImage Transform(Bitmap source)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                try
                {
                    source.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                    memory.Position = 0;
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = memory;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    return image;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }
    }
}
