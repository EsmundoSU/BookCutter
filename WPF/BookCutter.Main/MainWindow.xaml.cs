using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BookCutter.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true )
            {
                // Load basic image
                var imageBasicUri = new Uri(openFileDialog.FileName);
                BasicPhotoImage.Source = new BitmapImage(imageBasicUri);

                // Convert and load mask of photo
                var imageMaskMat = PhotoProcessing.FindBookMask(openFileDialog.FileName, (int)UpTresholdSlider.Value, (int)DownTresholdSlider.Value, (int)GaussianSlider.Value);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(imageMaskMat);

                // Cut mask form original photo
                var imageCutted = PhotoProcessing.CutBook(openFileDialog.FileName, imageMaskMat);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SinglePhotoRadioButton_Click(object sender, RoutedEventArgs e)
        {
            OpenPhotoButton.IsEnabled = true;
            SavePhotoButton.IsEnabled = true;

            OpenFolderButton.IsEnabled = false;
            SavePhotosButton.IsEnabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiplePhotosRadiobutton_Click(object sender, RoutedEventArgs e)
        {
            OpenPhotoButton.IsEnabled = false;
            SavePhotoButton.IsEnabled = false;

            OpenFolderButton.IsEnabled = true;
            SavePhotosButton.IsEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if(openFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var photosPathList = Directory.GetFiles(openFolderDialog.SelectedPath, "*.jpg").ToList();

                foreach (var photoPath in photosPathList)
                {
                    // Load basic image
                    var imageBasicUri = new Uri(photoPath);
                    BasicPhotoImage.Source = new BitmapImage(imageBasicUri);

                    // Convert and load mask of photo
                    var imageMaskMat = PhotoProcessing.FindBookMask(photoPath, (int)UpTresholdSlider.Value, (int)DownTresholdSlider.Value, (int)GaussianSlider.Value);
                    MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(imageMaskMat);

                    // Cut mask form original photo
                    var imageCutted = PhotoProcessing.CutBook(photoPath, imageMaskMat);
                    CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpTresholdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(BasicPhotoImage != null)
            {
                var basicImageImageSource = BasicPhotoImage.Source;
                var basicImageUri = new Uri(basicImageImageSource.ToString());
                var basicImagePath = basicImageUri.AbsolutePath;

                var downTresholdValue = (int)DownTresholdSlider.Value;
                var gaussianSizeValue = (int)GaussianSlider.Value;


                var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, (int)e.NewValue, downTresholdValue, gaussianSizeValue);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }

        private void DownTresholdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BasicPhotoImage != null)
            {
                var basicImageImageSource = BasicPhotoImage.Source;
                var basicImageUri = new Uri(basicImageImageSource.ToString());
                var basicImagePath = basicImageUri.AbsolutePath;

                var upTresholdValue = (int)UpTresholdSlider.Value;
                var gaussianSizeValue = (int)GaussianSlider.Value;

                var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, upTresholdValue, (int)e.NewValue, gaussianSizeValue);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }

        private void GaussianSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BasicPhotoImage != null)
            {
                var basicImageImageSource = BasicPhotoImage.Source;
                var basicImageUri = new Uri(basicImageImageSource.ToString());
                var basicImagePath = basicImageUri.AbsolutePath;

                var upTresholdValue = (int)UpTresholdSlider.Value;
                var downTresholdValue = (int)DownTresholdSlider.Value;

                var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, upTresholdValue, downTresholdValue, (int)e.NewValue);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }
    }
}
