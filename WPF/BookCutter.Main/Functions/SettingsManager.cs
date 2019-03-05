using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System;

namespace BookCutter.Main
{
    public static class SettingsManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetDefaultSettings()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            // Mask color settings after start-up
            if (settings[SettingsConstants.MaskColorKey] == null)
            {
                SettingsManager.AddUpdateAppSettings(SettingsConstants.MaskColorKey, SettingsConstants.MaskColorValueString);
                ((App)Application.Current).Settings.MaskColor = SettingsConstants.MaskColorValue;
            }
            else
            {
                Int32.TryParse(settings[SettingsConstants.MaskColorKey].Value, out int photoModeLoad);
                ((App)Application.Current).Settings.MaskColor = (MaskColorScale)photoModeLoad;
            }

            // AntiAliasing settings after start-up
            if (settings[SettingsConstants.AntiAliasingKey] == null)
            {
                SettingsManager.AddUpdateAppSettings(SettingsConstants.AntiAliasingKey, SettingsConstants.AntiAliasingValueString);
                ((App)Application.Current).Settings.AntiAliasing = SettingsConstants.AntiAliasingValue;
            }
            else
            {
                bool.TryParse(settings[SettingsConstants.AntiAliasingKey].Value, out bool antiAliasingIsChecked);
                ((App)Application.Current).Settings.AntiAliasing = antiAliasingIsChecked;
            }

            // UpTreshold value settings after start-up
            if (settings[SettingsConstants.UpTresholdKey] == null)
            {
                SettingsManager.AddUpdateAppSettings(SettingsConstants.UpTresholdKey, SettingsConstants.UpTresholdValueString);
                ((App)Application.Current).Settings.UpTresholdValue = SettingsConstants.UpTresholdValue;

            }
            else
            {
                Int32.TryParse(settings[SettingsConstants.UpTresholdKey].Value, out int upTresholdValue);
                ((App)Application.Current).Settings.UpTresholdValue = upTresholdValue;
            }

            // DownTreshold value settings after start-up
            if (settings[SettingsConstants.DownTresholdKey] == null)
            {
                SettingsManager.AddUpdateAppSettings(SettingsConstants.DownTresholdKey, SettingsConstants.DownTresholdValueString);
                ((App)Application.Current).Settings.DownTresholdValue = SettingsConstants.DownTresholdValue;
            }
            else
            {
                Int32.TryParse(settings[SettingsConstants.UpTresholdKey].Value, out int downTresholdValue);
                ((App)Application.Current).Settings.DownTresholdValue = downTresholdValue;
            }

            // Gaussian Size value setting after start-up
            if (settings[SettingsConstants.GaussianSizeKey] == null)
            {
                SettingsManager.AddUpdateAppSettings(SettingsConstants.GaussianSizeKey, SettingsConstants.GaussianSizeValueString);
                ((App)Application.Current).Settings.GaussianMatrixSize = SettingsConstants.GaussianSizeValue;
            }
            else
            {
                Int32.TryParse(settings[SettingsConstants.GaussianSizeKey].Value, out int gaussianSizeValue);
                ((App)Application.Current).Settings.GaussianMatrixSize = gaussianSizeValue;
            }

            // Photo mode state after start-up
            if (settings[SettingsConstants.PhotoModeLoadKey] == null)
            {
                SettingsManager.AddUpdateAppSettings(SettingsConstants.PhotoModeLoadKey, SettingsConstants.PhotoModeLoadValueString);
                ((App)Application.Current).Settings.PhotoModeLoad = SettingsConstants.PhotoModeLoadValue;
            }
            else
            {
                Int32.TryParse(settings[SettingsConstants.PhotoModeLoadKey].Value, out int photoModeLoad);
                ((App)Application.Current).Settings.PhotoModeLoad = (PhotoModeLoad)photoModeLoad;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoModeLoad"></param>
        public static void SetPhotoModeLoad(PhotoModeLoad photoModeLoad)
        {
            ((App)Application.Current).Settings.PhotoModeLoad = photoModeLoad;
            SettingsManager.AddUpdateAppSettings(SettingsConstants.PhotoModeLoadKey, ((int)photoModeLoad).ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PhotoModeLoad GetPhotoModeLoad()
        {
            return ((App)Application.Current).Settings.PhotoModeLoad;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="antiAliasing"></param>
        public static void SetAntiAliasingState(bool antiAliasing)
        {
            ((App)Application.Current).Settings.AntiAliasing = antiAliasing;
            SettingsManager.AddUpdateAppSettings(SettingsConstants.AntiAliasingKey, antiAliasing.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool GetAntiAliasingState()
        {
            return ((App)Application.Current).Settings.AntiAliasing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maskColorScale"></param>
        public static void SetMaskColor(MaskColorScale maskColorScale)
        {
            ((App)Application.Current).Settings.MaskColor = maskColorScale;
            SettingsManager.AddUpdateAppSettings(SettingsConstants.MaskColorKey, ((int)maskColorScale).ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MaskColorScale GetMaskColor()
        {
            return ((App)Application.Current).Settings.MaskColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upTresholdValue"></param>
        public static void SetUpTresholdValue(int upTresholdValue)
        {
            ((App)Application.Current).Settings.UpTresholdValue = upTresholdValue;
            SettingsManager.AddUpdateAppSettings(SettingsConstants.UpTresholdKey, upTresholdValue.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetUpTresholdValue()
        {
            return ((App)Application.Current).Settings.UpTresholdValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="downTresholdValue"></param>
        public static void SetDownTresholdValue(int downTresholdValue)
        {
            ((App)Application.Current).Settings.DownTresholdValue = downTresholdValue;
            SettingsManager.AddUpdateAppSettings(SettingsConstants.DownTresholdKey, downTresholdValue.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetDownTresholdValue()
        {
            return ((App)Application.Current).Settings.DownTresholdValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gaussianSize"></param>
        public static void SetGaussianSize(int gaussianSize)
        {
            ((App)Application.Current).Settings.GaussianMatrixSize = gaussianSize;
            SettingsManager.AddUpdateAppSettings(SettingsConstants.GaussianSizeKey, gaussianSize.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetGaussianSize()
        {
            return ((App)Application.Current).Settings.GaussianMatrixSize;
        }
    }
}
