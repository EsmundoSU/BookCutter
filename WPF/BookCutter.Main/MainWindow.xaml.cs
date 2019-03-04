using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            SettingsManager.ReadAllSettings();
            SettingsManager.AddUpdateAppSettings("Moje ustawienie1", "Adam Pelc");
            SettingsManager.ReadAllSettings();
            SettingsManager.AddUpdateAppSettings("Moje ustawienie2", "Adam Pelc3");
            SettingsManager.AddUpdateAppSettings("Moje ustawienie3", "Adam Pelc4");
            SettingsManager.ReadAllSettings();
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
                var imageMaskMat = PhotoProcessing.FindBookMask(openFileDialog.FileName, (int)UpTresholdSlider.Value, (int)DownTresholdSlider.Value, (int)GaussianSlider.Value, ColorScale.Gray);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(imageMaskMat);

                // Cut mask form original photo
                var imageCutted = PhotoProcessing.CutBookCV(openFileDialog.FileName, imageMaskMat, (bool)AntialiasingCheckBox.IsChecked);
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
                    var imageMaskMat = PhotoProcessing.FindBookMask(photoPath, (int)UpTresholdSlider.Value, (int)DownTresholdSlider.Value, (int)GaussianSlider.Value, ColorScale.Gray);
                    MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(imageMaskMat);

                    // Cut mask form original photo
                    var imageCutted = PhotoProcessing.CutBookCV(photoPath, imageMaskMat, (bool)AntialiasingCheckBox.IsChecked);
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


                var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, (int)e.NewValue, downTresholdValue, gaussianSizeValue, ColorScale.Gray);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat, (bool)AntialiasingCheckBox.IsChecked);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownTresholdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BasicPhotoImage != null)
            {
                var basicImageImageSource = BasicPhotoImage.Source;
                var basicImageUri = new Uri(basicImageImageSource.ToString());
                var basicImagePath = basicImageUri.AbsolutePath;

                var upTresholdValue = (int)UpTresholdSlider.Value;
                var gaussianSizeValue = (int)GaussianSlider.Value;

                var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, upTresholdValue, (int)e.NewValue, gaussianSizeValue, ColorScale.Gray);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat, (bool)AntialiasingCheckBox.IsChecked);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GaussianSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BasicPhotoImage != null)
            {
                var basicImageImageSource = BasicPhotoImage.Source;
                var basicImageUri = new Uri(basicImageImageSource.ToString());
                var basicImagePath = basicImageUri.AbsolutePath;

                var upTresholdValue = (int)UpTresholdSlider.Value;
                var downTresholdValue = (int)DownTresholdSlider.Value;

                var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, upTresholdValue, downTresholdValue, (int)e.NewValue, ColorScale.Gray);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat, (bool)AntialiasingCheckBox.IsChecked);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            if( CuttedPhotoImage.ActualHeight != 0 )
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Image (*.png)|*.png|JPG Image (*.jpg)|*.jpg";
                if( saveFileDialog.ShowDialog() == true )
                {
                    var savePath = saveFileDialog.FileName;
                    if( saveFileDialog.FilterIndex == 1 )
                    {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)CuttedPhotoImage.Source));
                        using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            encoder.Save(stream);
                        }
                    }
                    else if( saveFileDialog.FilterIndex == 2 )
                    {
                        var encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)CuttedPhotoImage.Source));
                        using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            encoder.Save(stream);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(BasicPhotoImage != null)
            {
                var basicImageImageSource = BasicPhotoImage.Source;
                var basicImageUri = new Uri(basicImageImageSource.ToString());
                var basicImagePath = basicImageUri.AbsolutePath;

                var upTresholdValue = (int)UpTresholdSlider.Value;
                var downTresholdValue = (int)DownTresholdSlider.Value;
                var gaussianSizeValue = (int)GaussianSlider.Value;

                var radioButton = (RadioButton)e.Source;

                ColorScale colorScale;

                if( radioButton == GrayMaskRadioButton )
                {
                    colorScale = ColorScale.Gray;
                }
                else if( radioButton == RedMaskRadioButton )
                {
                    colorScale = ColorScale.Red;
                }
                else if( radioButton == GreenMaskRadioButton )
                {
                    colorScale = ColorScale.Green;
                }
                else
                {
                    colorScale = ColorScale.Blue;
                }

                var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, upTresholdValue, downTresholdValue, gaussianSizeValue, colorScale);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat, (bool)AntialiasingCheckBox.IsChecked);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AntialiasingCheckBox_Change(object sender, RoutedEventArgs e)
        {
            if( BasicPhotoImage.ActualHeight != 0 )
            {
                var basicImageImageSource = BasicPhotoImage.Source;
                var basicImageUri = new Uri(basicImageImageSource.ToString());
                var basicImagePath = basicImageUri.AbsolutePath;

                var upTresholdValue = (int)UpTresholdSlider.Value;
                var downTresholdValue = (int)DownTresholdSlider.Value;
                var gaussianSizeValue = (int)GaussianSlider.Value;

                ColorScale colorScale;

                if ((bool)GrayMaskRadioButton.IsChecked)
                    colorScale = ColorScale.Gray;
                else if ((bool)RedMaskRadioButton.IsChecked)
                    colorScale = ColorScale.Red;
                else if ((bool)GreenMaskRadioButton.IsChecked)
                    colorScale = ColorScale.Green;
                else
                    colorScale = ColorScale.Green;

                var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, upTresholdValue, downTresholdValue, gaussianSizeValue, colorScale);
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat, (bool)AntialiasingCheckBox.IsChecked);
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);

            }
        }
    }
}
