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
            ((App)Application.Current).Settings = new SettingsModel();

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
            var appCurrent = (App)Application.Current;
            appCurrent.Images = new List<ImageModel>();
            appCurrent.SelectedImage = 0;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(openFileDialog.ShowDialog() == true )
            {
                appCurrent.Images.Add(new ImageModel
                {
                    ImagePath = openFileDialog.FileName,
                    ImageUri = new Uri(openFileDialog.FileName)
                });

                #region Image Processing
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
                #endregion

                #region GUI Changes
                CurrentPhotoPageLabel.Content = "1";
                AllPhotosPagesLabel.Content = "1";
                #endregion
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
            appCurrent.SelectedImage = 0;

            // Open folder browser
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImagesManager.AddImagesFromFolderPath(folderBrowser.SelectedPath);

                // If more than one photo unlock bottom menu
                if (appCurrent.Images.Count > 1)
                {
                    RightArrowButton.IsEnabled = true;
                    CurrentPhotoPageLabel.Content = "1";
                    AllPhotosPagesLabel.Content = appCurrent.Images.Count.ToString();
                }

                #region Image Processing
                // Load basic image
                BasicPhotoImage.Source = new BitmapImage(appCurrent.Images[0].ImageUri);

                // Convert and load mask of photo
                var imageMaskMat = PhotoProcessing.FindBookMask(
                    appCurrent.Images[0].ImagePath,
                    SettingsManager.GetUpTresholdValue(),
                    SettingsManager.GetDownTresholdValue(),
                    SettingsManager.GetGaussianSize(),
                    SettingsManager.GetMaskColor());
                MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(imageMaskMat);

                // Cut mask form original photo
                var imageCutted = PhotoProcessing.CutBookCV(
                    appCurrent.Images[0].ImagePath,
                    imageMaskMat,
                    SettingsManager.GetAntiAliasingState());
                CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
                #endregion
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            if (CuttedPhotoImage.ActualHeight != 0)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG Image (*.png)|*.png|JPG Image (*.jpg)|*.jpg";
                if (saveFileDialog.ShowDialog() == true)
                {
                    var savePath = saveFileDialog.FileName;
                    if (saveFileDialog.FilterIndex == 1)
                    {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)CuttedPhotoImage.Source));
                        using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            encoder.Save(stream);
                        }
                    }
                    else if (saveFileDialog.FilterIndex == 2)
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
        private void PhotoModeRadioButton_Click(object sender, RoutedEventArgs e)
        {
            var radioButton = (RadioButton)e.Source;

            if (radioButton == SinglePhotoRadioButton)
            {
                SettingsManager.SetPhotoModeLoad(PhotoModeLoad.Single);

                OpenFolderButton.IsEnabled = false;
                SavePhotosButton.IsEnabled = false;
                OpenPhotoButton.IsEnabled = true;
                SavePhotoButton.IsEnabled = true;
            }
            else if (radioButton == MultiplePhotosRadiobutton)
            {
                SettingsManager.SetPhotoModeLoad(PhotoModeLoad.Multiple);

                OpenFolderButton.IsEnabled = true;
                SavePhotosButton.IsEnabled = true;
                OpenPhotoButton.IsEnabled = false;
                SavePhotoButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var appCurrent = (App)Application.Current;
            var slider = (Slider)e.Source;

            // Setting new value for sliders
            if (slider.Name == "UpTresholdSlider")
                SettingsManager.SetUpTresholdValue((int)e.NewValue);
            else if (slider.Name == "DownTresholdSlider")
                SettingsManager.SetDownTresholdValue((int)e.NewValue);
            else if (slider.Name == "GaussianSlider")
                SettingsManager.SetGaussianSize((int)e.NewValue);

            if (BasicPhotoImage != null)
            {
                if (BasicPhotoImage.ActualHeight > 0)
                {
                    #region Image Processing
                    var maskImageMat = PhotoProcessing.FindBookMask(
                        appCurrent.Images[appCurrent.SelectedImage].ImagePath,
                        SettingsManager.GetUpTresholdValue(),
                        SettingsManager.GetDownTresholdValue(),
                        SettingsManager.GetGaussianSize(),
                        SettingsManager.GetMaskColor());
                    MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                    var imageCutted = PhotoProcessing.CutBookCV(
                        appCurrent.Images[appCurrent.SelectedImage].ImagePath,
                        maskImageMat,
                        SettingsManager.GetAntiAliasingState());
                    CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
                    #endregion
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
            var appCurrent = (App)Application.Current;
            var radioButton = (RadioButton)e.Source;

            // Setting new value for mask color checkbox
            if (radioButton.Name == "GrayMaskRadioButton")
                SettingsManager.SetMaskColor(MaskColorScale.Gray);
            else if (radioButton.Name == "RedMaskRadioButton")
                SettingsManager.SetMaskColor(MaskColorScale.Red);
            else if (radioButton.Name == "GreenMaskRadioButton")
                SettingsManager.SetMaskColor(MaskColorScale.Green);
            else if (radioButton.Name == "BlueMaskRadioButton")
                SettingsManager.SetMaskColor(MaskColorScale.Blue);

            if (BasicPhotoImage != null)
            {
                if (BasicPhotoImage.ActualHeight > 0)
                {
                    #region Image Processing
                    var maskImageMat = PhotoProcessing.FindBookMask(
                        appCurrent.Images[appCurrent.SelectedImage].ImagePath,
                        SettingsManager.GetUpTresholdValue(),
                        SettingsManager.GetDownTresholdValue(),
                        SettingsManager.GetGaussianSize(),
                        SettingsManager.GetMaskColor());
                    MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                    var imageCutted = PhotoProcessing.CutBookCV(
                        appCurrent.Images[appCurrent.SelectedImage].ImagePath,
                        maskImageMat,
                        SettingsManager.GetAntiAliasingState());
                    CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
                    #endregion
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
            SettingsManager.SetAntiAliasingState((bool)AntialiasingCheckBox.IsChecked);

            var appCurrent = (App)Application.Current;

            if (BasicPhotoImage != null)
            {
                if (BasicPhotoImage.ActualHeight > 0)
                {
                    #region Image Processing
                    var maskImageMat = PhotoProcessing.FindBookMask(
                        appCurrent.Images[appCurrent.SelectedImage].ImagePath,
                        SettingsManager.GetUpTresholdValue(),
                        SettingsManager.GetDownTresholdValue(),
                        SettingsManager.GetGaussianSize(),
                        SettingsManager.GetMaskColor());
                    MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(maskImageMat);

                    var imageCutted = PhotoProcessing.CutBookCV(
                        appCurrent.Images[appCurrent.SelectedImage].ImagePath,
                        maskImageMat,
                        SettingsManager.GetAntiAliasingState());
                    CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
                    #endregion
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArrowButton_Click(object sender, RoutedEventArgs e)
        {
            var appCurrent = (App)Application.Current;
            var button = (Button)e.Source;

            #region Interface Change
            if (button.Name == "RightArrowButton" & appCurrent.SelectedImage < appCurrent.Images.Count - 1 )
            {
                appCurrent.SelectedImage += 1;
                CurrentPhotoPageLabel.Content = (appCurrent.SelectedImage + 1).ToString();
            }
            else if (button.Name == "LeftArrowButton" & appCurrent.SelectedImage > 0)
            {
                appCurrent.SelectedImage -= 1;
                CurrentPhotoPageLabel.Content = (appCurrent.SelectedImage + 1).ToString();
            }

            if (button.Name == "LeftArrowButton" & appCurrent.SelectedImage == 0)
                LeftArrowButton.IsEnabled = false;
            else
                LeftArrowButton.IsEnabled = true;

            if (button.Name == "RightArrowButton" & appCurrent.SelectedImage == appCurrent.Images.Count - 1)
                RightArrowButton.IsEnabled = false;
            else
                RightArrowButton.IsEnabled = true;
            #endregion

            #region Image Processing
            // Load basic image
            BasicPhotoImage.Source = new BitmapImage(appCurrent.Images[appCurrent.SelectedImage].ImageUri);

            // Convert and load mask of photo
            var imageMaskMat = PhotoProcessing.FindBookMask(
                appCurrent.Images[appCurrent.SelectedImage].ImagePath,
                SettingsManager.GetUpTresholdValue(),
                SettingsManager.GetDownTresholdValue(),
                SettingsManager.GetGaussianSize(),
                SettingsManager.GetMaskColor());
            MaskPhotoImage.Source = PhotoProcessing.MatToImageSource(imageMaskMat);

            // Cut mask form original photo
            var imageCutted = PhotoProcessing.CutBookCV(
                appCurrent.Images[appCurrent.SelectedImage].ImagePath,
                imageMaskMat,
                SettingsManager.GetAntiAliasingState());
            CuttedPhotoImage.Source = PhotoProcessing.MatToImageSource(imageCutted);
            #endregion
        }
    }
}
