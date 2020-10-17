using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace CrashNSaneLoadDetector
{
	//This class contains settings, features and methods for computing features from a given Bitmap
	internal class FeatureDetector
	{
		#region Public Fields

		//this list of vectors is for 300x100, patchSize = 50, numberOfBins = 16
		//to adapt - if wrongly detected pause, increase match threshold and add wrongly detected runs to list
		public static int[,] listOfFeatureVectorsEng = { { 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

		public static int[,] listOfFeatureVectorsTransition = {
			{2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2500,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			{ 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

		public static int numberOfBinsCorrect = 537;

		#endregion Public Fields

		#region Private Fields

		private static float additiveVariance = 2.0f;

		public static int numberOfBins = 16;

		public static int patchSizeX = 50;

		public static int patchSizeY = 50;

		//used as a cutoff for when a match is detected correctly
		private static float varianceOfBinsAllowed = 1.0f;

		private static float varianceOfBinsAllowedMult = 1.45f;

		#endregion Private Fields

		#region Public Methods

    public static void rgb2hsv(int r, int g, int b, out int h, out int s, out int v)
    {
      double r_normalized = r / 255.0f;
      double g_normalized = g / 255.0f;
      double b_normalized = b / 255.0f;

      // h, s, v = hue, saturation, value 
      double cmax = Math.Max(r_normalized, Math.Max(g_normalized, b_normalized)); // maximum of r, g, b 
      double cmin = Math.Min(r_normalized, Math.Min(g_normalized, b_normalized)); // minimum of r, g, b 
      double diff = cmax - cmin; // diff of cmax and cmin. 
      double h_d = -1;
      double s_d = -1;

      // if cmax and cmax are equal then h = 0 
      if (cmax == cmin)
        h_d = 0;

      // if cmax equal r then compute h 
      else if (cmax == r_normalized)
        h_d = (60 * ((g_normalized - b_normalized) / diff) + 360) % 360;

      // if cmax equal g then compute h 
      else if (cmax == g_normalized)
        h_d = (60 * ((b_normalized - r_normalized) / diff) + 120) % 360;

      // if cmax equal b then compute h 
      else if (cmax == b_normalized)
        h_d = (60 * ((r_normalized - g_normalized) / diff) + 240) % 360;

      // if cmax equal zero 
      if (cmax == 0)
        s_d = 0;
      else
        s_d = (diff / cmax) * 100;

      // compute v 
      double v_d = cmax * 100;

      h = Convert.ToInt32(h_d);
      s = Convert.ToInt32(s_d);
      v = Convert.ToInt32(v_d);
    }

    public class HSVRange
    {
      public int h_min;
      public int h_max;
      public int s_min;
      public int s_max;
      public int v_min;
      public int v_max;

      public HSVRange(int hmin, int hmax, int smin, int smax, int vmin, int vmax)
      {
        h_min = hmin;
        h_max = hmax;
        s_min = smin;
        s_max = smax;
        v_min = vmin;
        v_max = vmax;
      }
    }



    public static void compareImageCaptureCrash4(Bitmap capture, List<HSVRange> hsv_ranges, List<int> gradient_thresholds, List<float> achieved_hsv_ranges, List<float> achieved_gradient_thresholds, List<float> average_thresholded_gradients, int gradient_color_channel_offset)
    {
      List<int> number_of_pixels_in_range = new List<int>(hsv_ranges.Count);
      List<int> number_of_gradient_pixels_in_threshold = new List<int>(gradient_thresholds.Count);

      for (int i = 0; i < hsv_ranges.Count; i++)
      {
        achieved_hsv_ranges[i] = 0;
        number_of_pixels_in_range.Add(0);
      }

      for (int i = 0; i < gradient_thresholds.Count; i++)
      {
        achieved_gradient_thresholds[i] = 0;
        average_thresholded_gradients[i] = 0;
        number_of_gradient_pixels_in_threshold.Add(0);
      }

      BitmapData bData = capture.LockBits(new Rectangle(0, 0, capture.Width, capture.Height), ImageLockMode.ReadWrite, capture.PixelFormat);
      int bmpStride = bData.Stride;
      int size = bData.Stride * bData.Height;

      byte[] data = new byte[size];

      /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
      System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);
      int yAdd = 0;
      int r = 0;
      int g = 0;
      int b = 0;
      int h = 0;
      int s = 0;
      int v = 0;
      //we look at 50x50 patches and compute histogram bins for the a/r/g/b values.

      int stride = 1; //spacing between feature pixels

      for (int patchX = 0; patchX < (capture.Width / patchSizeX); patchX++)
      {
        for (int patchY = 0; patchY < (capture.Height / patchSizeY); patchY++)
        {
          int xStart = patchX * (patchSizeX * stride);
          int yStart = patchY * (patchSizeX * stride);
          int xEnd = (patchX + 1) * (patchSizeX * stride);
          int yEnd = (patchY + 1) * (patchSizeY * stride);

          for (int x_index = xStart; x_index < xEnd; x_index += stride)
          {
            for (int y_index = yStart; y_index < yEnd; y_index += stride)
            {
              yAdd = y_index * bmpStride;

              //NOTE: while the pixel format is 32ARGB, reading byte-wise results in BGRA.
              b = (int)(data[(x_index * 4) + (yAdd) + 0]);
              g = (int)(data[(x_index * 4) + (yAdd) + 1]);
              r = (int)(data[(x_index * 4) + (yAdd) + 2]);

              rgb2hsv(r, g, b, out h, out s, out v);

              // sobel filter on blue color channel
              if(gradient_thresholds.Count != 0)
              {
                if (x_index < (bData.Width - 1) && x_index > 0 && y_index < (bData.Height - 1) && y_index > 0)
                {
                  int sobel_grad = -(int)(data[((x_index - 1) * 4) + (y_index - 1) * bmpStride + gradient_color_channel_offset])
                    - 2 * (int)(data[((x_index - 1) * 4) + (y_index) * bmpStride + gradient_color_channel_offset])
                    - (int)(data[((x_index - 1) * 4) + (y_index + 1) * bmpStride + gradient_color_channel_offset])
                    + (int)(data[((x_index + 1) * 4) + (y_index - 1) * bmpStride + gradient_color_channel_offset])
                    + 2 * (int)(data[((x_index + 1) * 4) + (y_index) * bmpStride + gradient_color_channel_offset])
                    + (int)(data[((x_index + 1) * 4) + (y_index + 1) * bmpStride + gradient_color_channel_offset]);

                  sobel_grad = Math.Abs(sobel_grad);

                  // We do the following with sobel:
                  // 1.) Compute the number of pixels (relative to img size) above threshold
                  // 2.) Compute the average gradient above threshold

                  for(int i = 0; i < gradient_thresholds.Count; i++)
                  {
                    if(sobel_grad > gradient_thresholds[i])
                    {
                      number_of_gradient_pixels_in_threshold[i]++;
                      average_thresholded_gradients[i] += sobel_grad;
                    }
                  }

                }
              }
              


              for (int i = 0; i < hsv_ranges.Count; i++)
              {               
                if (h >= hsv_ranges[i].h_min && h <= hsv_ranges[i].h_max && s >= hsv_ranges[i].s_min && s <= hsv_ranges[i].s_max && v >= hsv_ranges[i].v_min && v <= hsv_ranges[i].v_max)
                {
                  number_of_pixels_in_range[i]++;
                }
              }
              

            }
          }

        }
      }

      capture.UnlockBits(bData);

      for (int i = 0; i < hsv_ranges.Count; i++)
      {
        achieved_hsv_ranges[i] = number_of_pixels_in_range[i] / Convert.ToSingle(capture.Width * capture.Height);
      }

      for (int i = 0; i < gradient_thresholds.Count; i++)
      {
        achieved_gradient_thresholds[i] = number_of_gradient_pixels_in_threshold[i] / Convert.ToSingle(capture.Width * capture.Height);
        average_thresholded_gradients[i] /= number_of_gradient_pixels_in_threshold[i];
      }

    }

    public static bool compareImageCaptureHSVCrash4(Bitmap capture, float detection_threshold, out float achieved_threshold, int h_min, int h_max, int s_min, int s_max, int v_min, int v_max)
    {
      int number_of_pixels = 0;
      
      BitmapData bData = capture.LockBits(new Rectangle(0, 0, capture.Width, capture.Height), ImageLockMode.ReadWrite, capture.PixelFormat);
      int bmpStride = bData.Stride;
      int size = bData.Stride * bData.Height;

      byte[] data = new byte[size];

      /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
      System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);
      int yAdd = 0;
      int r = 0;
      int g = 0;
      int b = 0;
      int h = 0;
      int s = 0;
      int v = 0;
      //we look at 50x50 patches and compute histogram bins for the a/r/g/b values.

      int stride = 1; //spacing between feature pixels

      for (int patchX = 0; patchX < (capture.Width / patchSizeX); patchX++)
      {
        for (int patchY = 0; patchY < (capture.Height / patchSizeY); patchY++)
        {
          int xStart = patchX * (patchSizeX * stride);
          int yStart = patchY * (patchSizeX * stride);
          int xEnd = (patchX + 1) * (patchSizeX * stride);
          int yEnd = (patchY + 1) * (patchSizeY * stride);

          for (int x_index = xStart; x_index < xEnd; x_index += stride)
          {
            for (int y_index = yStart; y_index < yEnd; y_index += stride)
            {
              yAdd = y_index * bmpStride;

              //NOTE: while the pixel format is 32ARGB, reading byte-wise results in BGRA.
              b = (int)(data[(x_index * 4) + (yAdd) + 0]);
              g = (int)(data[(x_index * 4) + (yAdd) + 1]);
              r = (int)(data[(x_index * 4) + (yAdd) + 2]);

              rgb2hsv(r, g, b, out h, out s, out v);

              if(h >= h_min && h <= h_max && s >= s_min && s <= s_max && v >= v_min && v <= v_max)
              {
                number_of_pixels++;
              }


            }
          }

        }
      }

      capture.UnlockBits(bData);

      achieved_threshold = number_of_pixels / Convert.ToSingle(capture.Width * capture.Height);

      return achieved_threshold >= detection_threshold;
    }

		public static bool compareFeatureVector(int[] newVector, int[,] comparison_vectors, out int matchingBins, float percentageOfBinsCorrectOverride = -1.0f, bool debugOutput = true)
		{
			//int[,] comparison_vectors = listOfFeatureVectorsEng;
			int size = newVector.Length;

			if (comparison_vectors.GetLength(1) < size)
			{
				size = comparison_vectors.GetLength(1);
			}

			//int number_of_bins_needed = 290;// (int) (size * percent_of_bins_correct);

			int numVectors = comparison_vectors.GetLength(0);

			matchingBins = 0;
			int matching = 0;
			int matching_bins_result = 0;
			Parallel.For(0, numVectors,
			  (vectorIndex, loopState) =>
				{
					int tempMatchingBins = 0;
			  //check if the current feature vector matches one of the stored ones closely enough

			  for (int bin = 0; bin < size; bin++)
					{
				  //Determine upper/lower histogram ranges for matching bins
				  int lower_bound = (int)((comparison_vectors[vectorIndex, bin] / varianceOfBinsAllowedMult) - additiveVariance);
						int upper_bound = (int)((comparison_vectors[vectorIndex, bin] * varianceOfBinsAllowedMult) + additiveVariance);

						if (newVector[bin] <= upper_bound && newVector[bin] >= lower_bound)
						{
							tempMatchingBins++;
						}

				  //If we can not get a possible match anymore, break for speed
				  if (((bin - tempMatchingBins) > (size - numberOfBinsCorrect)) && percentageOfBinsCorrectOverride < 0.0f)
						{
							break;
						}
					}


					if (tempMatchingBins >= numberOfBinsCorrect)
					{
				  // if we found enough similarities, we found a match. this will early-out for speed, but possibly report lower matchingBins values
				  // we also swap this comparison index with the first one, to give it priority.
				  //int temp = vectorIndex;
				  //int swap_position = i / 4;
				  // comparison_indices[i] = comparison_indices[swap_position];
				  //comparison_indices[swap_position] = vectorIndex;
				  Interlocked.Exchange(ref matching, 1);
						Interlocked.Exchange(ref matching_bins_result, tempMatchingBins);
						loopState.Stop();
					}
				}
			  );

			matchingBins = matching_bins_result;
			if (matching != 0)
				return true;

			if (debugOutput)
			{
				System.Console.WriteLine("Matching bins: " + matchingBins);
			}

			if (matchingBins >= numberOfBinsCorrect && percentageOfBinsCorrectOverride < 0.0f)
			{
				//if we found enough similarities, we found a match.
				return true;
			}

			if (percentageOfBinsCorrectOverride >= 0.0f)
			{
				System.Console.WriteLine("Matching bins (percent): " + (matchingBins / (float)size));
				System.Console.WriteLine("Required bins (percent): " + percentageOfBinsCorrectOverride);
			}

			if (percentageOfBinsCorrectOverride >= 0.0f && (matchingBins / (float)size) >= percentageOfBinsCorrectOverride)
			{
				return true;
			}

			return false;
		}

		public static bool compareFeatureVector(int[] newVector, List<int[]> comparison_vectors, out int matchingBins, float percentageOfBinsCorrectOverride = -1.0f, bool debugOutput = true)
		{
			//int[,] comparison_vectors = listOfFeatureVectorsEng;
			int size = newVector.Length;

			matchingBins = 0;

			if (comparison_vectors.Count == 0)
				return false;

			if (comparison_vectors[0].Length < size)
			{
				size = comparison_vectors[0].Length;
			}

			//int number_of_bins_needed = 290;// (int) (size * percent_of_bins_correct);

			int numVectors = comparison_vectors.Count;

			int matching = 0;
			int matching_bins_result = 0;
			Parallel.For(0, numVectors,
			  (vectorIndex, loopState) =>
			  {
				  int tempMatchingBins = 0;
			//check if the current feature vector matches one of the stored ones closely enough

			for (int bin = 0; bin < size; bin++)
				  {
				//Determine upper/lower histogram ranges for matching bins
				int lower_bound = (int)((comparison_vectors[vectorIndex][bin] / varianceOfBinsAllowedMult) - additiveVariance);
					  int upper_bound = (int)((comparison_vectors[vectorIndex][bin] * varianceOfBinsAllowedMult) + additiveVariance);

					  if (newVector[bin] <= upper_bound && newVector[bin] >= lower_bound)
					  {
						  tempMatchingBins++;
					  }

				//If we can not get a possible match anymore, break for speed
				if (((bin - tempMatchingBins) > (size - numberOfBinsCorrect)) && percentageOfBinsCorrectOverride < 0.0f)
					  {
						  break;
					  }
				  }


				  if (tempMatchingBins >= numberOfBinsCorrect)
				  {
				// if we found enough similarities, we found a match. this will early-out for speed, but possibly report lower matchingBins values
				// we also swap this comparison index with the first one, to give it priority.
				//int temp = vectorIndex;
				//int swap_position = i / 4;
				// comparison_indices[i] = comparison_indices[swap_position];
				//comparison_indices[swap_position] = vectorIndex;
				Interlocked.Exchange(ref matching, 1);
					  Interlocked.Exchange(ref matching_bins_result, tempMatchingBins);
					  loopState.Stop();
				  }
			  }
			  );

			matchingBins = matching_bins_result;
			if (matching != 0)
				return true;

			if (debugOutput)
			{
				System.Console.WriteLine("Matching bins: " + matchingBins);
			}

			if (matchingBins >= numberOfBinsCorrect && percentageOfBinsCorrectOverride < 0.0f)
			{
				//if we found enough similarities, we found a match.
				return true;
			}

			if (percentageOfBinsCorrectOverride >= 0.0f)
			{
				System.Console.WriteLine("Matching bins (percent): " + (matchingBins / (float)size));
				System.Console.WriteLine("Required bins (percent): " + percentageOfBinsCorrectOverride);
			}

			if (percentageOfBinsCorrectOverride >= 0.0f && (matchingBins / (float)size) >= percentageOfBinsCorrectOverride)
			{
				return true;
			}

			return false;
		}

		public static bool isGameTransition(Bitmap capture, int black_level)
		{
			BitmapData bData = capture.LockBits(new Rectangle(0, 0, capture.Width, capture.Height), ImageLockMode.ReadWrite, capture.PixelFormat);
			int bmpStride = bData.Stride;
			int size = bData.Stride * bData.Height;

			byte[] data = new byte[size];

			/*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
			System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);
			int yAdd = 0;
			int r = 0;
			int g = 0;
			int b = 0;
			//we look at 50x50 patches and compute histogram bins for the a/r/g/b values.

			int stride = 1; //spacing between feature pixels



			for (int patchX = 0; patchX < (capture.Width / patchSizeX); patchX++)
			{
				for (int patchY = 0; patchY < (capture.Height / patchSizeY); patchY++)
				{
					int xStart = patchX * (patchSizeX * stride);
					int yStart = patchY * (patchSizeX * stride);
					int xEnd = (patchX + 1) * (patchSizeX * stride);
					int yEnd = (patchY + 1) * (patchSizeY * stride);

					for (int x_index = xStart; x_index < xEnd; x_index += stride)
					{
						for (int y_index = yStart; y_index < yEnd; y_index += stride)
						{
							yAdd = y_index * bmpStride;

							//NOTE: while the pixel format is 32ARGB, reading byte-wise results in BGRA.
							b += (int)(data[(x_index * 4) + (yAdd) + 0]);
							g += (int)(data[(x_index * 4) + (yAdd) + 1]);
							r += (int)(data[(x_index * 4) + (yAdd) + 2]);


						}
					}
				}
			}


			capture.UnlockBits(bData);


			b /= (capture.Width * capture.Height);
			r /= (capture.Width * capture.Height);
			g /= (capture.Width * capture.Height);


			return (b < black_level && r < black_level && g < black_level);
		}


		private static bool checkBlackLevelTransition(int[] newVector, int[,] comparison_vectors, List<int> max_per_patch, List<int> min_per_patch, float max_transition_threshold, out float average_max_transition, out int matchingBins, float percentageOfBinsCorrectOverride = -1.0f, bool debugOutput = true)
		{
			//int[,] comparison_vectors = listOfFeatureVectorsEng;
			int size = newVector.Length;

			if (comparison_vectors.GetLength(1) < size)
			{
				size = comparison_vectors.GetLength(1);
			}

			//int number_of_bins_needed = 290;// (int) (size * percent_of_bins_correct);

			int numVectors = comparison_vectors.GetLength(0);

			// For the transitions, we want to check if the screen is black, that is the sum of the lowest 3 bins (color range from 0 - (256/numberOfBins) * 3 -> 0 - 48
			// This should be robust enough unless people have got some serious issues with their black in their captures

			matchingBins = 0;

			int number_of_black_bins = 4;

			int black_level = (256 / numberOfBins) * 2;

			if (max_transition_threshold > 0)
			{
				black_level = Convert.ToInt32(max_transition_threshold);
			}

			int max_max = 0;
			average_max_transition = 0.0f;


			int transition_tolerance = 2;


			foreach (int max_val in max_per_patch)
			{
				max_max = Math.Max(max_val, max_max);
				average_max_transition += max_val;
			}

			// Average of patch-max values for black level calibration
			average_max_transition = average_max_transition / max_per_patch.Count;

			// Baseline: If the *maximum* of all pixels is less than the tolerance, we can immediately decide that it is a transition.
			if (max_max < transition_tolerance + 2)
				return true;

			//Console.WriteLine("Black Level {0}", max_max_transition);

			// If we have a max_transition_threshold given from averaging, we can say that it is a transition if we are below the threshold with a given tolerance.
			if (max_transition_threshold > 0 && average_max_transition <= (max_transition_threshold + transition_tolerance))
			{
				//Console.WriteLine("Detected max {0} > {1}, no transition!", max_max_transition, max_transition_threshold + transition_tolerance);
				return true;
			}
			else
			{
				// Additional check in case the black level isn't calibrated yet
				// Here we can only ensure if we are *not* in a transition because we're checking max values for each patch. 
				foreach (int max_val in max_per_patch)
				{
					if (max_val > black_level)
					{
						//Console.WriteLine("Detected max {0} > {1}, no transition!", max_val, black_level);
						return false;
					}
				}
			}


			// Console.WriteLine("Detected max {0} <= {1}, might transition!", max_max, black_level);
			int num_total_pixels_per_patch = patchSizeX * patchSizeY;

			float percentage_correct = ((num_total_pixels_per_patch) * percentageOfBinsCorrectOverride);
			int matching_patches = 0;
			int total_patches = 0;
			int similarity_additive_difference = 100;
			// Iterate over each histogram
			for (int bin = 0; bin < size; bin += (numberOfBins * 3))
			{
				int sum_red = 0;
				int sum_green = 0;
				int sum_blue = 0;
				bool rgb_similarity = true;

				for (int black_bin_offset = 0; black_bin_offset < number_of_black_bins; black_bin_offset++)
				{
					int r = newVector[bin + black_bin_offset];
					int g = newVector[bin + numberOfBins + black_bin_offset];
					int b = newVector[bin + 2 * numberOfBins + black_bin_offset];
					sum_red += r;
					sum_green += g;
					sum_blue += b;

					int min_rgb = Math.Min(b + similarity_additive_difference, Math.Min(r + similarity_additive_difference, g + similarity_additive_difference));
					int max_rgb = Math.Max(b - similarity_additive_difference, Math.Max(r - similarity_additive_difference, g - similarity_additive_difference));

					if ((r < min_rgb && r > max_rgb && g < min_rgb && g > max_rgb && b < min_rgb && b > max_rgb) == false)
					{
						rgb_similarity = false;
					}

				}

				total_patches++;

				// Check if the distribution matches, also check if all channels are similar in range
				if (sum_red >= percentage_correct
				  && sum_green >= percentage_correct
				  && sum_blue >= percentage_correct
				  && rgb_similarity == true
				  )
				{
					matching_patches++;
				}
			}

			//Console.WriteLine("Transition: Matching {0}, Total {1}", matching_patches, total_patches);
			if (matching_patches == total_patches)
			{
				return true;
			}


			// Finally, only if we haven't calibrated yet, we check the patch max for a conservative detection

			if (max_transition_threshold < 0)
			{
				if (max_max < (black_level + 1))
					return true;
			}


			return false;
		}

		private static bool checkWhiteLevelTransition(int[] newVector, int[,] comparison_vectors, List<int> max_per_patch, List<int> min_per_patch, float max_transition_threshold, out float average_max_transition, out int matchingBins, float percentageOfBinsCorrectOverride = -1.0f, bool debugOutput = true)
		{
			//int[,] comparison_vectors = listOfFeatureVectorsEng;
			int size = newVector.Length;

			if (comparison_vectors.GetLength(1) < size)
			{
				size = comparison_vectors.GetLength(1);
			}

			//int number_of_bins_needed = 290;// (int) (size * percent_of_bins_correct);

			int numVectors = comparison_vectors.GetLength(0);

			// For the transitions, we want to check if the screen is black, that is the sum of the lowest 3 bins (color range from 0 - (256/numberOfBins) * 3 -> 0 - 48
			// This should be robust enough unless people have got some serious issues with their black in their captures

			matchingBins = 0;

			int number_of_white_bins = 4;

			int white_level = (256 / numberOfBins) * (numberOfBins - 2);

			if (max_transition_threshold > 0)
			{
				white_level = Convert.ToInt32(max_transition_threshold);
			}

			int min_min = 9999;
			average_max_transition = 0.0f;


			int transition_tolerance = 253;


			foreach (int min_val in min_per_patch)
			{
				min_min = Math.Min(min_val, min_min);
				average_max_transition += min_min;
			}

			// Average of patch-max values for black level calibration
			average_max_transition = average_max_transition / max_per_patch.Count;

			// Baseline: If the *maximum* of all pixels is less than the tolerance, we can immediately decide that it is a transition.
			if (min_min > transition_tolerance - 2)
				return true;

			//Console.WriteLine("Black Level {0}", max_max_transition);

			// If we have a max_transition_threshold given from averaging, we can say that it is a transition if we are below the threshold with a given tolerance.
			if (max_transition_threshold > 0 && average_max_transition <= (max_transition_threshold + transition_tolerance))
			{
				//Console.WriteLine("Detected max {0} > {1}, no transition!", max_max_transition, max_transition_threshold + transition_tolerance);
				return true;
			}
			else
			{
				// Additional check in case the black level isn't calibrated yet
				// Here we can only ensure if we are *not* in a transition because we're checking max values for each patch. 
				foreach (int min_val in min_per_patch)
				{
					if (min_val < white_level)
					{
						//Console.WriteLine("Detected max {0} > {1}, no transition!", max_val, black_level);
						return false;
					}
				}
			}


			// Console.WriteLine("Detected max {0} <= {1}, might transition!", max_max, black_level);
			int num_total_pixels_per_patch = patchSizeX * patchSizeY;

			float percentage_correct = ((num_total_pixels_per_patch) * percentageOfBinsCorrectOverride);
			int matching_patches = 0;
			int total_patches = 0;
			int similarity_additive_difference = 100;
			// Iterate over each histogram
			for (int bin = 0; bin < size; bin += (numberOfBins * 3))
			{
				int sum_red = 0;
				int sum_green = 0;
				int sum_blue = 0;
				bool rgb_similarity = true;

				for (int white_bin_offset = numberOfBins - number_of_white_bins; white_bin_offset < number_of_white_bins; white_bin_offset++)
				{
					int r = newVector[bin + white_bin_offset];
					int g = newVector[bin + numberOfBins + white_bin_offset];
					int b = newVector[bin + 2 * numberOfBins + white_bin_offset];
					sum_red += r;
					sum_green += g;
					sum_blue += b;

					int min_rgb = Math.Min(b + similarity_additive_difference, Math.Min(r + similarity_additive_difference, g + similarity_additive_difference));
					int max_rgb = Math.Max(b - similarity_additive_difference, Math.Max(r - similarity_additive_difference, g - similarity_additive_difference));

					if ((r < min_rgb && r > max_rgb && g < min_rgb && g > max_rgb && b < min_rgb && b > max_rgb) == false)
					{
						rgb_similarity = false;
					}

				}

				total_patches++;

				// Check if the distribution matches, also check if all channels are similar in range
				if (sum_red >= percentage_correct
				  && sum_green >= percentage_correct
				  && sum_blue >= percentage_correct
				  && rgb_similarity == true
				  )
				{
					matching_patches++;
				}
			}

			//Console.WriteLine("Transition: Matching {0}, Total {1}", matching_patches, total_patches);
			if (matching_patches == total_patches)
			{
				return true;
			}


			// Finally, only if we haven't calibrated yet, we check the patch min for a conservative detection

			if (max_transition_threshold < 0)
			{
				if (min_min > (white_level - 1))
					return true;
			}


			return false;
		}

		public static bool compareFeatureVectorTransition(int[] newVector, int[,] comparison_vectors, List<int> max_per_patch, List<int> min_per_patch, float max_transition_threshold, out float average_max_transition, out int matchingBins, float percentageOfBinsCorrectOverride = -1.0f, bool debugOutput = true)
		{

			if (checkBlackLevelTransition(newVector, comparison_vectors, max_per_patch, min_per_patch, max_transition_threshold, out average_max_transition, out matchingBins, percentageOfBinsCorrectOverride, debugOutput))
				return true;

			if (checkWhiteLevelTransition(newVector, comparison_vectors, max_per_patch, min_per_patch, max_transition_threshold, out average_max_transition, out matchingBins, percentageOfBinsCorrectOverride, debugOutput))
				return true;

			return false;
		}

		public static List<int> featuresFromBitmap(Bitmap capture, out List<int> max_per_patch, out int black_level, out List<int> min_per_patch)
		{
			List<int> features = new List<int>();
			max_per_patch = new List<int>();
			min_per_patch = new List<int>();
			black_level = 255;
			BitmapData bData = capture.LockBits(new Rectangle(0, 0, capture.Width, capture.Height), ImageLockMode.ReadWrite, capture.PixelFormat);
			int bmpStride = bData.Stride;
			int size = bData.Stride * bData.Height;

			byte[] data = new byte[size];

			/*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
			System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);
			int yAdd = 0;
			int r = 0;
			int g = 0;
			int b = 0;
			//we look at 50x50 patches and compute histogram bins for the a/r/g/b values.

			int stride = 1; //spacing between feature pixels

			for (int patchX = 0; patchX < (capture.Width / patchSizeX); patchX++)
			{
				for (int patchY = 0; patchY < (capture.Height / patchSizeY); patchY++)
				{
					//int[] patch_hist_a = new int[numberOfBins];
					int[] patchHistR = new int[numberOfBins];
					int[] patchHistG = new int[numberOfBins];
					int[] patchHistB = new int[numberOfBins];

					int xStart = patchX * (patchSizeX * stride);
					int yStart = patchY * (patchSizeX * stride);
					int xEnd = (patchX + 1) * (patchSizeX * stride);
					int yEnd = (patchY + 1) * (patchSizeY * stride);

					int b_max = 0;
					int g_max = 0;
					int r_max = 0;
					int b_min = 9999;
					int g_min = 9999;
					int r_min = 9999;

					for (int x_index = xStart; x_index < xEnd; x_index += stride)
					{
						for (int y_index = yStart; y_index < yEnd; y_index += stride)
						{
							yAdd = y_index * bmpStride;

							//NOTE: while the pixel format is 32ARGB, reading byte-wise results in BGRA.
							b = (int)(data[(x_index * 4) + (yAdd) + 0]);
							g = (int)(data[(x_index * 4) + (yAdd) + 1]);
							r = (int)(data[(x_index * 4) + (yAdd) + 2]);

							black_level = Math.Min(black_level, (r + g + b) / 3);


							b_max = Math.Max(b, b_max);
							g_max = Math.Max(g, g_max);
							r_max = Math.Max(r, r_max);

							b_min = Math.Min(b, b_min);
							g_min = Math.Min(g, g_min);
							r_min = Math.Min(r, r_min);

							patchHistR[(r * numberOfBins) / 256]++;
							patchHistG[(g * numberOfBins) / 256]++;
							patchHistB[(b * numberOfBins) / 256]++;
						}
					}

					max_per_patch.Add(b_max);
					max_per_patch.Add(g_max);
					max_per_patch.Add(r_max);

					min_per_patch.Add(b_min);
					min_per_patch.Add(g_min);
					min_per_patch.Add(r_min);

					//enter the histograms as our features
					features.AddRange(patchHistR);
					features.AddRange(patchHistG);
					features.AddRange(patchHistB);
				}
			}

			capture.UnlockBits(bData);

			return features;
		}

		#endregion Public Methods
	}
}