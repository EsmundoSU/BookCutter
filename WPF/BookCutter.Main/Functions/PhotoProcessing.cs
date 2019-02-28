using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace BookCutter.Main
{
    internal static class PhotoProcessing
    {
        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject([In] IntPtr hObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static Mat FindBookMask(string filePath)
        {
            var imgBasic = Cv2.ImRead(filePath);
            //Cv2.ImShow("Debug(CV2) - Basic Image", imgBasic);

            var imgGray = imgBasic.Clone();
            Cv2.CvtColor(imgBasic, imgGray, ColorConversionCodes.BGR2GRAY);
            //Cv2.ImShow("Debug(CV2) - Grey Image", imgGray);

            var imgGaussian = imgGray.Clone();
            Cv2.GaussianBlur(imgGray, imgGaussian, new OpenCvSharp.Size(3, 3), 0);
            //Cv2.ImShow("Debug(CV2) - Gaussian", imgGaussian);

            var imgCanny = imgGaussian.Clone();
            Cv2.Canny(imgGaussian, imgCanny, 150, 10);
            //Cv2.ImShow("Debug(CV2) - Canny", imgCanny);

            var imgClosed = imgCanny.Clone();
            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(7, 7));
            Cv2.MorphologyEx(imgCanny, imgClosed, MorphTypes.Close, kernel);
            //Cv2.ImShow("Debug(CV2) - Closed", imgClosed);

            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchyIndexes;
            Cv2.FindContours(imgClosed, out contours, out hierarchyIndexes, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

            var contourMaxArea = 0.0;
            var contourMaxIndex = 0;
            for (int i = 0; i < contours.Length; i++)
            {
                var contour = contours[i];
                if (Cv2.ContourArea(contour) > contourMaxArea)
                {
                    contourMaxArea = Cv2.ContourArea(contour);
                    contourMaxIndex = i;
                }
            }

            var imgContour = (Mat)Mat.Zeros(imgClosed.Size(), MatType.CV_8UC1);
            Cv2.DrawContours(imgContour, contours, contourMaxIndex, new Scalar(255), -1);
            //Cv2.ImShow("Debig(CV2) - Counters", imgContour);

            return imgContour;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceImageFilePath"></param>
        /// <param name="imageMaskMat"></param>
        /// <returns></returns>
        internal static Mat CutBook(string sourceImageFilePath, Mat imageMaskMat)
        {
            var imageBasicMat = Cv2.ImRead(sourceImageFilePath);
            for (int i = 0; i < imageBasicMat.Cols; i++)
            {
                for (int j = 0; j < imageBasicMat.Rows; j++)
                {
                    if (imageMaskMat.At<Vec3b>(j, i)[0] == 0)
                        imageBasicMat.Set(j, i, new Vec3b(255, 255, 255));
                }
            }

            return imageBasicMat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageMat"></param>
        /// <returns></returns>
        internal static Bitmap MatToBitmap(Mat imageMat)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(imageMat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageBitmap"></param>
        /// <returns></returns>
        internal static Mat BitmapToMat(Bitmap imageBitmap)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToMat(imageBitmap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        internal static ImageSource BitmapToImageSource(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageImageSource"></param>
        /// <returns></returns>
        internal static Bitmap ImageSourceToBitmap(ImageSource imageImageSource)
        {
            var imageBitmapSource = (BitmapSource)imageImageSource;
            var width = imageBitmapSource.PixelWidth;
            var height = imageBitmapSource.PixelHeight;
            var stride = width * ((imageBitmapSource.Format.BitsPerPixel + 7) / 8);
            var ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.AllocHGlobal(height * width);
                imageBitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), ptr, height * stride, stride);

                using (var btm = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, ptr))
                {
                    return new Bitmap(btm);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageMat"></param>
        /// <returns></returns>
        internal static ImageSource MatToImageSource(Mat imageMat)
        {
            var imgBitmap = MatToBitmap(imageMat);
            return BitmapToImageSource(imgBitmap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageImageSource"></param>
        /// <returns></returns>
        internal static Mat ImageSourceToMat(ImageSource imageImageSource)
        {
            var imageBitmap = ImageSourceToBitmap(imageImageSource);
            return BitmapToMat(imageBitmap);
        }

    }
}
