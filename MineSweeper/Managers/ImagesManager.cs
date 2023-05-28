using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace MineSweeper.Managers
{
    public static class ImagesManager
    {
        public static List<ImageSource> ImagesList;
        static string[] ImagePaths;
        public static void LoadImages()
        {
            ImagesList = new List<ImageSource>();
            // the images folder needs to be placed in the same directory as the executable file
            string imagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            ImagePaths = Directory.GetFiles(imagesPath, "*.jpg");
            foreach (var imagePath in ImagePaths)
            {
                ImagesList.Add(new BitmapImage(new Uri(imagePath)));
            }
        }
    }
}
