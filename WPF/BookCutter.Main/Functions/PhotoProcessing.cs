using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.Win32;

namespace BookCutter.Main
{
    internal static class PhotoProcessing
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Method to convert Bitmap to BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private static BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            try
            {
                retval = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }

        internal static BitmapImage TestFunction()
        {
            BitmapImage myPicture2 = new BitmapImage();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            { 
                var uri = new Uri(openFileDialog.FileName);
                myPicture2 = new BitmapImage(uri);

                var img = CvInvoke.Imread(openFileDialog.FileName);
                CvInvoke.Imshow("Debug - Basic Image", img);

                var imgGray = img.Clone();
                CvInvoke.CvtColor(img, imgGray, ColorConversion.Bgr2Gray);
                CvInvoke.Imshow("Debug - Grey", imgGray);

                var imgGaussian = img.Clone();
                var gaussianSize = new System.Drawing.Size(3, 3);
                CvInvoke.GaussianBlur(imgGray, imgGaussian, gaussianSize, 0);
                CvInvoke.Imshow("Debug - Gaussian", imgGaussian);

                var imgCanny = img.Clone();
                CvInvoke.Canny(imgGaussian, imgCanny, 150, 10);
                CvInvoke.Imshow("Debug - Canny", imgCanny);

                var elementSize = new System.Drawing.Size(7, 7);
                var elementAnchor = new System.Drawing.Point(-1, -1);
                var kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, elementSize, elementAnchor);
                var imgClosed = img.Clone();
                CvInvoke.MorphologyEx(imgCanny, imgClosed, MorphOp.Close, kernel, elementAnchor, 1, BorderType.Constant, new MCvScalar() );
                CvInvoke.Imshow("Debug - Closed", imgClosed);

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imgClosed, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);


                // TODO: Znalezienie najwiekszej kontury zdjecia
                var contoursArray = contours.ToArrayOfArray();
                foreach (var contourArray in contoursArray)
                {
                    Debug.Write("Dlugosc elementu: ");
                    Debug.WriteLine(contourArray.Length);
                }

                var imgMask = imgClosed.Clone();
                CvInvoke.DrawContours(imgMask, contours, 0, new MCvScalar(255, 0, 0));
                CvInvoke.Imshow("Debug - Mask", imgMask);
                
            }

            return myPicture2;
        }

    }
}
