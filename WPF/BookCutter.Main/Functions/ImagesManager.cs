using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BookCutter.Main
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImagesManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void AddImagesFromFolderPath(string path)
        {
            var appCurrent = (App)Application.Current;

            // Gather photos from path to arrays (*.JPG | *.JPEG | *.PNG )
            string[] imagesJpgPaths = Directory.GetFiles(path, "*.jpg");
            string[] imagesJpegPaths = Directory.GetFiles(path, "*.jpeg");
            string[] imagesPngPaths = Directory.GetFiles(path, "*.png");

            // Add all (*.JPEG) to image list
            foreach (var imagePath in imagesJpgPaths)
            {
                appCurrent.Images.Add(new ImageModel
                {
                    ImagePath = imagePath,
                    ImageUri = new Uri(imagePath),
                    ImageName = Path.GetFileName(imagePath)
                });
            }

            // Add all (*.JPEG) to image list
            foreach (var imagePath in imagesJpegPaths)
            {
                appCurrent.Images.Add(new ImageModel
                {
                    ImagePath = imagePath,
                    ImageUri = new Uri(imagePath),
                    ImageName = Path.GetFileName(imagePath)
                });
            }

            // Add all (*.PNG) to image list
            foreach (var imagePath in imagesPngPaths)
            {
                appCurrent.Images.Add(new ImageModel
                {
                    ImagePath = imagePath,
                    ImageUri = new Uri(imagePath),
                    ImageName = Path.GetFileName(imagePath)
                });
            }
        }
    }
}
