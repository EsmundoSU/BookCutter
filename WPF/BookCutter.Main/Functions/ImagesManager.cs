using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookCutter.Main
{
    public static class ImagesManager
    {
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
                    ImageUri = new Uri(imagePath)
                });
            }

            // Add all (*.JPEG) to image list
            foreach (var imagePath in imagesJpegPaths)
            {
                appCurrent.Images.Add(new ImageModel
                {
                    ImagePath = imagePath,
                    ImageUri = new Uri(imagePath)
                });
            }

            // Add all (*.PNG) to image list
            foreach (var imagePath in imagesPngPaths)
            {
                appCurrent.Images.Add(new ImageModel
                {
                    ImagePath = imagePath,
                    ImageUri = new Uri(imagePath)
                });
            }
        }
    }
}
