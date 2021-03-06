﻿using System.Collections.Generic;
using System.Windows;

namespace BookCutter.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Global access to photos from app scope
        /// </summary>
        public List<ImageModel> Images { get; set; }

        /// <summary>
        /// Indicates which image is now processing
        /// </summary>
        public int SelectedImage { get; set; }

        /// <summary>
        /// Global access to app setting from app scope
        /// </summary>
        public SettingsModel Settings { get; set; }
    }
}
