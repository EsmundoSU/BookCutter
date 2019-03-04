namespace BookCutter.Main
{
    public class SettingsModel
    {
        /// <summary>
        /// Indicaties if program load multiple or one photo
        /// </summary>
        public PhotoModeLoad PhotoModeLoad { get; set; }

        /// <summary>
        /// Select which color scale photo should be processed with
        /// </summary>
        public ColorScale ColorScale { get; set; }

        /// <summary>
        /// Indicates if antiAliasing option for photo is turned On/Off
        /// </summary>
        public bool AntiAliasing { get; set; }

        /// <summary>
        /// Upper treshold for Canny Edge Detection value
        /// </summary>
        public int UpTresholdValue { get; set; }

        /// <summary>
        /// Down treshold for Canny Edge Detection value
        /// </summary>
        public int DownTresholdValue { get; set; }

        /// <summary>
        /// Gaussian rectangle matrix size
        /// </summary>
        public int GaussianMatrixSize { get; set; }
    }
}
