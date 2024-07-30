using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

namespace climb_higher;

// ================================================
// resources used:
// see: https://www.youtube.com/watch?v=0ibBYinRiEA
// see: https://www.youtube.com/watch?v=cMJwqxskyek
// see: https://stackoverflow.com/questions/36817133/identifying-the-range-of-a-color-in-hsv-using-opencv
// ================================================

class ComputerVision
{
    private static int MAX_RECT_SIDE_LENGTH = 500;

    /// <summary>
    /// Given an input image and a color range, this method identifies objects within that color
    /// range and returns a marked-up image with rectangles around instances of that color.
    /// </summary>
    /// <param name="pathToInputImage">File path to the input image</param>
    /// <param name="lowerColorLimit">The Hsv value representing the lower bound of the color range</param>
    /// <param name="upperColorLimit">The Hsv value representing the upper bound of the color range</param>
    /// <returns>A base64 string representation of the input image with blue rectangles
    /// surrounding objects within the given color range</returns>
    public static string PerformAnalysis(string pathToInputImage, Hsv lowerColorLimit, Hsv upperColorLimit)
    {
        // read in image and convert to HSV version
        Image<Bgr, Byte> inputImage = new Image<Bgr, Byte>(pathToInputImage);
        inputImage.SmoothGaussian(5);
        Image<Hsv, Byte> hsvImage = inputImage.Convert<Hsv, Byte>();

        // define blue color using HSV (used for rectangles on the image)
        // Hue - [0, 179]
        // Saturation - [0, 255]
        // Value/Brightness - [0, 255]
        Hsv blueColor = new Hsv(120, 255, 255);

        // for the given feature (color of hold), find similar features in image using a color range and create a mask
        var maskForRect = hsvImage.InRange(lowerColorLimit, upperColorLimit);

        // find coordinates of instances of that color
        VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
        Mat hierarchy = new Mat();
        // Contours are essentially just a curve of points. In this case, the points form an
        // edge around a hold based on its color feature. Contours are created using edge detection, among other algorithms.
        CvInvoke.FindContours(maskForRect, contours, hierarchy, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

        // for each hold found, draw a rectangle around it.
        // rectangle uses blue color defined above, although the color choice is arbitrary
        for (int i = 0; i < contours.Size; i++)
        {
            if (CvInvoke.ContourArea(contours[i]) > 500)
            {
                Rectangle contourRect = CvInvoke.BoundingRectangle(contours[i]);
                contourRect.Inflate(50, 50);
                if (contourRect.Width <= MAX_RECT_SIDE_LENGTH &&
                    contourRect.Height <= MAX_RECT_SIDE_LENGTH)
                {
                    DrawRectangleOnImage(contourRect, hsvImage, blueColor, 20);
                }
            }
        }

        // convert the image back to Bgr format and return the base64 string
        // representation of the image so it can be read in by an Image XAML element
        Image<Bgr, Byte> outputImage = hsvImage.Convert<Bgr, Byte>();
        byte[] bytes = outputImage.ToJpegData();
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Draws a rectangle on the given image.
    /// </summary>
    /// <param name="rect">The rectangle to draw</param>
    /// <param name="image">The image to draw the rectangle on</param>
    /// <param name="color">The color of the rectangle</param>
    /// <param name="thickness">The thickness of the rectangle's lines</param>
    private static void DrawRectangleOnImage(Rectangle rect, Image<Hsv, Byte> image, Hsv color, int thickness)
    {
        image.Draw(rect, color, thickness);
    }

    /// <summary>
    /// Computes the lower and upper HSV color bounds for a color range based on the given BGR color
    /// </summary>
    /// <param name="bgrColor">A BGR color to find the HSV range of</param>
    /// <returns>An array of Hsv values where the first item is
    /// the lower bound and the second item is the upper bound</returns>
    public static Hsv[]? ComputeColorRanges(Bgr bgrColor)
    {
        Image<Bgr, Byte> bgrInput = new Image<Bgr, Byte>(1, 1, bgrColor);
        Image<Hsv, Byte> hsvOutput = new Image<Hsv, Byte>(1, 1);

        CvInvoke.CvtColor(bgrInput, hsvOutput, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
        int hue = hsvOutput.Data[0, 0, 0];

        int lowerSaturation, lowerValue, upperSaturation, upperValue;
        lowerSaturation = lowerValue = 100;
        upperSaturation = upperValue = 255;
        int offset = 10;
        int lowerHue, upperHue;
        lowerHue = hue - offset;
        upperHue = hue + offset;

        if (lowerHue - offset < 0)
        {
            lowerHue = 0;
        }

        Hsv lowerRange = new Hsv(lowerHue, lowerSaturation, lowerValue);
        Hsv upperRange = new Hsv(upperHue, upperSaturation, upperValue);
        return new Hsv[] { lowerRange, upperRange };
    }

    /// <summary>
    /// Converts a Hsv value into .NET MAUI's representation of a color
    /// </summary>
    /// <param name="hsvVal">The Hsv value to convert into a MAUI color</param>
    /// <returns>A MAUI specific color equivalent to the given Hsv value</returns>
    public static Microsoft.Maui.Graphics.Color HsvToColor(Hsv hsvVal)
    {
        Image<Hsv, Byte> hsvInput = new Image<Hsv, Byte>(1, 1, hsvVal);
        Image<Bgr, Byte> bgrOutput = new Image<Bgr, Byte>(1, 1);

        CvInvoke.CvtColor(hsvInput, bgrOutput, Emgu.CV.CvEnum.ColorConversion.Hsv2Bgr);
        Bgr bgrVal = bgrOutput.GetAverage();
        return new Microsoft.Maui.Graphics.Color((int)bgrVal.Red, (int)bgrVal.Green, (int)bgrVal.Blue);
    }

    /// <summary>
    /// Converts a Bgr value into a Hsv value.
    /// </summary>
    /// <param name="bgrInput">The Bgr value to convert.</param>
    /// <returns>Returns a Hsv representation of the Bgr value.</returns>
    public static Hsv BgrToHsv(Bgr bgrInput)
    {
        Image<Bgr, Byte> bgrImg = new Image<Bgr, Byte>(1, 1, bgrInput);
        Image<Hsv, Byte> hsvImg = new Image<Hsv, Byte>(1, 1);

        CvInvoke.CvtColor(bgrImg, hsvImg, Emgu.CV.CvEnum.ColorConversion.Bgr2Hsv);
        return hsvImg.GetAverage();
    }
}
