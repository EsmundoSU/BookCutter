using System;

namespace BookCutter.Main
{
    public class ImageModel
    {
        /// <summary>
        /// Store URI of picture/image
        /// </summary>
        public Uri ImageUri { get; set; }

        /// <summary>
        /// Store image path
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Store only name of Image from Path
        /// </summary>
        public string ImageName { get; set; }
    }
}
