using System.Configuration;
using System.Diagnostics;
using System;

namespace BookCutter.Main
{
    internal static class SettingsManager
    {
        internal static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationSettings.AppSettings;

                if (appSettings.Count == 0)
                {
                    Debug.WriteLine("AppSettings is empty");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Debug.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationException)
            {
                Debug.WriteLine("Error reading app settings");
            }
        }
    }
}
