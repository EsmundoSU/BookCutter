namespace BookCutter.Main
{
    public static class SettingsConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public static string MaskColorKey { get { return "MaskColor"; } }

        /// <summary>
        /// 
        /// </summary>
        public static MaskColorScale MaskColorValue { get { return MaskColorScale.Gray; } }

        /// <summary>
        /// 
        /// </summary>
        public static string MaskColorValueString { get { return ((int)MaskColorValue).ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        public static string AntiAliasingKey { get { return "AntiAliasing"; } }

        /// <summary>
        /// 
        /// </summary>
        public static bool AntiAliasingValue { get { return false; } }

        /// <summary>
        /// 
        /// </summary>
        public static string AntiAliasingValueString { get { return AntiAliasingValue.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        public static string UpTresholdKey { get { return "UpTreshold"; } }

        /// <summary>
        /// 
        /// </summary>
        public static int UpTresholdValue { get { return 150; } }

        /// <summary>
        /// 
        /// </summary>
        public static string UpTresholdValueString { get { return UpTresholdValue.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        public static string DownTresholdKey { get { return "DownTreshold"; } }

        /// <summary>
        /// 
        /// </summary>
        public static int DownTresholdValue { get { return 20; } }

        /// <summary>
        /// 
        /// </summary>
        public static string DownTresholdValueString { get { return DownTresholdValue.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        public static string GaussianSizeKey { get { return "GaussianSize"; } }

        /// <summary>
        /// 
        /// </summary>
        public static int GaussianSizeValue { get { return 3; } }

        /// <summary>
        /// 
        /// </summary>
        public static string GaussianSizeValueString { get { return GaussianSizeValue.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        public static string PhotoModeLoadKey { get { return "PhotoModeLoad"; } }

        /// <summary>
        /// 
        /// </summary>
        public static PhotoModeLoad PhotoModeLoadValue { get { return PhotoModeLoad.Single; } }

        /// <summary>
        /// 
        /// </summary>
        public static string PhotoModeLoadValueString { get { return ((int)PhotoModeLoadValue).ToString(); } }
    }
}
