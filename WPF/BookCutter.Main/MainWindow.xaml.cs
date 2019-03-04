using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Collections.Generic;


namespace BookCutter.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            SettingsManager.SetDefaultSettings();

            #region Setting Photo Load Mode
            var photoModeLoad = SettingsManager.GetPhotoModeLoad();

            switch (photoModeLoad)
            {
                case PhotoModeLoad.Single:
                    SinglePhotoRadioButton.IsChecked = true;
                    OpenFolderButton.IsEnabled = false;
                    SavePhotosButton.IsEnabled = false;
                    OpenPhotoButton.IsEnabled = true;
                    SavePhotoButton.IsEnabled = true;
                    break;
                case PhotoModeLoad.Multiple:
                    MultiplePhotosRadiobutton.IsChecked = true;
                    OpenFolderButton.IsEnabled = true;
                    SavePhotosButton.IsEnabled = true;
                    OpenPhotoButton.IsEnabled = false;
                    SavePhotoButton.IsEnabled = false;
                    break;
            }
            #endregion

            #region Setting Mask Radioboxes
            var maskColor = SettingsManager.GetMaskColor();

            switch (maskColor)
            {
                case MaskColorScale.Gray:
                    GrayMaskRadioButton.IsChecked = true;
                    break;
                case MaskColorScale.Red:
                    RedMaskRadioButton.IsChecked = true;
                    break;
                case MaskColorScale.Green:
                    GreenMaskRadioButton.IsChecked = true;
                    break;
                case MaskColorScale.Blue:
                    BlueMaskRadioButton.IsChecked = true;
                    break;
            }
            #endregion

            #region Setting Antialiasing Checkbox
            AntialiasingCheckBox.IsChecked = SettingsManager.GetAntiAliasingState();
            #endregion

            #region Setting UpTreshold Slider Value
            UpTresholdSlider.Value = SettingsManager.GetUpTresholdValue();
            #endregion

            #region Setting DownTreshold Slider Value
            DownTresholdSlider.Value = SettingsManager.GetDownTresholdValue();
            #endregion

            #region Setting Gaussian Size Value
            GaussianSlider.Value = SettingsManager.GetGaussianSize();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var appCurrent = (App)Application.Current;
            appCurrent.Images = new List<ImageModel>();

            if(openFileDialog.ShowDialog() == true )
            {
                appCurrent.Images.Add(new ImageModel
                {
                    ImagePath = openFileDialog.FileName,
                    ImageUri = new Uri(openFileDialog.FileName)
                });

                // Set basic image
                var imageBasicUri = new Uri(openFileDialog.FileName);
                BasicPhotoImage.Source = new BitmapImage(imageBasicUri);

                // Convert and Load mask of photo
                var imageMaskMat = PhotoProcessing.FindBookMask(
                    openFileDialog.FileName,
                    SettingsManager.GetUpTresholdValue(), 
                    SettingsManager.GetDownTresholdValue(),
                    SettingsManager.GetGaussianSize(),
                    SettingsManager.GetMaskColor());
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(imageMaskMat);

                // Cut mask from original photo
                var imageCutted = PhotoProcessing.CutBookCV(
                    openFileDialog.FileName, 
                    imageMaskMat, 
                    SettingsManager.GetAntiAliasingState());
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Initialize new list of photos
            var appCurrent = (App)Application.Current;
            appCurrent.Images = new List<ImageModel>();

            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            var selectedPath = "";

            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = folderBrowser.SelectedPath;
            }

            Debug.WriteLine("Selected path: {0}", selectedPath, null);

            // Gather photos
            string[] imagesJpgPaths = Directory.GetFiles(selectedPath, "*.jpg");
            string[] imagesJpegPaths = Directory.GetFiles(selectedPath, "*.jpeg");
            string[] imagesPngPaths = Directory.GetFiles(selectedPath, "*.png");

            // How many photos has been 
            var amountOfPhotos = imagesJpegPaths.Length + imagesJpgPaths.Length + imagesPngPaths.Length;
            Debug.WriteLine("Amount of pictures found: {0}", amountOfPhotos);

            if (amountOfPhotos > 1)
            {
                RightArrowButton.IsEnabled = true;
                CurrentPhotoPageLabel.Content = "1";
                AllPhotosPagesLabel.Content = amountOfPhotos.ToString();
            }

            // Adding photos path to global variable of photo
            ((App)Application.Current).Photos = new List<ImageModel>();

            foreach (var imageJpgPath in imagesJpegPaths)
            {
                ((App)Application.Current).Photos.Add(new ImageModel
                {
                    ImagePath = imageJpgPath
                });
            }

            foreach (var imageJpegPath in imagesJpegPaths)
            {
                ((App)Application.Current).Photos.Add(new ImageModel
                {
                    ImagePath = imageJpegPath
                });
            }

            foreach (var imagePngPath in imagesPngPaths)
            {
                ((App)Application.Current).Photos.Add(new ImageModel
                {
                    ImagePath = imagePngPath
                });
            }

            foreach (var photo in ((App)Application.Current).Photos)
            {
                photo.ImageUri = new Uri(photo.ImagePath);
            }

            // Load basic image
            BasicPhotoImage.Source = new BitmapImage((Application.Current as App).Photos[0].ImageUri);

            ColorScale colorScale;

            if ((bool)GrayMaskRadioButton.IsChecked)
            {
                colorScale = ColorScale.Gray;
            }
            else if ((bool)RedMaskRadioButton.IsChecked)
            {
                colorScale = ColorScale.Red;
            }
            else if ((bool)GreenMaskRadioButton.IsChecked)
            {
                colorScale = ColorScale.Green;
            }
            else
            {
                colorScale = ColorScale.Blue;
            }

            // Convert and load mask of photo
            var imageMaskMat = PhotoProcessing.FindBookMask((Application.Current as App).Photos[0].ImagePath, (int)UpTresholdSlider.Value, (int)DownTresholdSlider.Value, (int)GaussianSlider.Value, colorScale);
            MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(imageMaskMat);

            // Cut mask form original photo
            var imageCutted = PhotoProcessing.CutBookCV((Application.Current as App).Photos[0].ImagePath, imageMaskMat, (bool)AntialiasingCheckBox.IsChecked);
            CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
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
        private void UpTresholdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(BasicPhotoImage != null)
            {
                if(BasicPhotoImage.ActualHeight > 0 )
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

            // Update app settings with new value
            SettingsManager.AddUpdateAppSettings(SettingsManager.UpTresholdKey, e.NewValue.ToString());
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
                if( BasicPhotoImage.ActualHeight > 0 )
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
            
            // Update app settings with new value
            SettingsManager.AddUpdateAppSettings(SettingsManager.DownTresholdKey, e.NewValue.ToString());
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
                if( BasicPhotoImage.ActualHeight > 0 )
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

                // Update app settings with new value
                SettingsManager.AddUpdateAppSettings(SettingsManager.GaussianSizeKey, e.NewValue.ToString());
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
            var radioButton = (RadioButton)e.Source;

            ColorScale colorScale;

            if (radioButton == GrayMaskRadioButton)
            {
                colorScale = ColorScale.Gray;   
            }
            else if (radioButton == RedMaskRadioButton)
            {
                colorScale = ColorScale.Red;
            }
            else if (radioButton == GreenMaskRadioButton)
            {
                colorScale = ColorScale.Green;
            }
            else
            {
                colorScale = ColorScale.Blue;
            }

            // Update app settings for color mask
            SettingsManager.AddUpdateAppSettings(SettingsManager.MaskColorKey, ((int)colorScale).ToString());

            if (BasicPhotoImage != null)
            {
                if(BasicPhotoImage.ActualHeight > 0 )
                {
                    var basicImageImageSource = BasicPhotoImage.Source;
                    var basicImageUri = new Uri(basicImageImageSource.ToString());
                    var basicImagePath = basicImageUri.AbsolutePath;

                    var upTresholdValue = (int)UpTresholdSlider.Value;
                    var downTresholdValue = (int)DownTresholdSlider.Value;
                    var gaussianSizeValue = (int)GaussianSlider.Value;

                    var maskImageMat = PhotoProcessing.FindBookMask(basicImagePath, upTresholdValue, downTresholdValue, gaussianSizeValue, colorScale);
                    MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                    var imageCutted = PhotoProcessing.CutBookCV(basicImagePath, maskImageMat, (bool)AntialiasingCheckBox.IsChecked);
                    CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
                }
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

            SettingsManager.AddUpdateAppSettings(SettingsManager.AntiAliasingKey, ((bool)AntialiasingCheckBox.IsChecked).ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhotoModeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = (RadioButton)e.Source;

            PhotoModeLoad photoModeLoad = 0;

            if(radioButton == SinglePhotoRadioButton)
            {
                photoModeLoad = PhotoModeLoad.Single;
                OpenFolderButton.IsEnabled = false;
                SavePhotosButton.IsEnabled = false;
                OpenPhotoButton.IsEnabled = true;
                SavePhotoButton.IsEnabled = true;
            }
            else if( radioButton == MultiplePhotosRadiobutton)
            {
                photoModeLoad = PhotoModeLoad.Multiple;
                OpenFolderButton.IsEnabled = true;
                SavePhotosButton.IsEnabled = true;
                OpenPhotoButton.IsEnabled = false;
                SavePhotoButton.IsEnabled = false;
            }

            SettingsManager.AddUpdateAppSettings(SettingsManager.PhotoModeLoadKey, ((int)photoModeLoad).ToString());
        }
    }
}
