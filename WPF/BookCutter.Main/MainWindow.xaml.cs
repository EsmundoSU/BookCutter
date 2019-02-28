using Microsoft.Win32;
using System;
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
                var imageMaskMat = PhotoProcessing.FindBookMask(openFileDialog.FileName);
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
                    var imageMaskMat = PhotoProcessing.FindBookMask(photoPath);
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
            if(BasicPhotoImage.Source != null)
            {
                var basicImage = BasicPhotoImage.Source;
            }
        }
    }
}
