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
        public static string MaskColorValue { get { return ((int)ColorScale.Gray).ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        public static string AntiAliasingKey { get { return "AntiAliasing"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string AntiAliasingValue { get { return false.ToString(); } }

        /// <summary>
        /// 
        /// </summary>
        public static string UpTresholdKey { get { return "UpTreshold"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string UpTresholdValue { get { return "150"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string DownTresholdKey { get { return "DownTreshold"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string DownTresholdValue { get { return "20"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string GaussianSizeKey { get { return "GaussianSize"; } }

        /// <summary>
        /// 
        /// </summary>
        public static string PhotoModeLoadKey { get { return "PhotoModeLoad"; } }
    }
}
