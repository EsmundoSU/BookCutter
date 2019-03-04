using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCutter.Main
{
    public class ImageParametersModel
    {
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
