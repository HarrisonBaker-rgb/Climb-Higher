//using Android.Gestures;
using Emgu.CV.Structure;
using SkiaSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace climb_higher;
public partial class CameraPage : ContentPage
{
    static Image DisplayPhotoImage;
    static FileResult photo;
    SKBitmap bitmap;
    double bMapScaleX = 1;
    double bMapScaleY = 1;
    static MemoryStream imageDecodeStream;
    private int lowerRed;
    private int upperRed;
    private int lowerGreen;
    private int upperGreen;
    private int lowerBlue;
    private int upperBlue;
    const int RGB_MAX_VALUE = 255;
    public static bool firstImageTap = true;

    // Zoom variables.
    double currentScale = 1;
    double startScale = 1;
    double xOffset = 0;
    double yOffset = 0;

    /// <summary>
    /// Camera Page Function
    /// </summary>
    public CameraPage()
    {
        InitializeComponent();
        DisplayPhotoImage = DisplayPhoto;

        //Could be a good idea to edit this if we ever consider implementing IOS or other platforms
        //If IOS implemented probably just make it similar to android code (do nothing)
#if     __ANDROID__
        //Do nothing
#else
                DisplayPhoto.HeightRequest = 500;
#endif
        PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += PinchUpdated;
        // DisplayPhoto.GestureRecognizers.Add(pinchGesture);
    }
    /// <summary>
    /// When the page is opened we do these things.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        firstImageTap = true;
        imageDecodeStream = new MemoryStream();
        DisplayPhoto.Source = "";
    }
    /// <summary>
    /// Handles operations upon page being popped from navigation stack.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        belowImageLayout.IsVisible = false;
        imageDecodeStream = null;
    }
    /// <summary>
    /// Allows the user to take a picture on the camera page for the
    /// CV operations.
    /// </summary>
    /// <param name="sender">Object responsible for the event.</param>
    /// <param name="e">Eventargs for this event.</param>
    async void Camera_Button_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                DisplayPhoto.Source = photo.FullPath;
            }

            using (var stream = await photo.OpenReadAsync())
            {
                bitmap = SKBitmap.Decode(stream);
            }

            DisplayPhotoImage = DisplayPhoto;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Broke");
        }
    }
    /// <summary>
    /// Opens the user's gallery to select an image to use for cv operations.
    /// </summary>
    /// <param name="sender"> The object responsible for the event.</param>
    /// <param name="e">The eventargs for the event.</param>
    async void Pick_Button_Clicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo != null)
            {
                DisplayPhoto.Source = photo.FullPath;
            }

            using (var stream = await photo.OpenReadAsync())
            {
                bitmap = SKBitmap.Decode(stream);
            }
            DisplayPhotoImage = DisplayPhoto;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Broke");
        }
    }

    /// <summary>
    /// onImgTap() allows user to tap on the image and get the color based on 
    /// where the user tapped
    /// </summary>
    /// <param name="sender"> sender in this case is the image </param>
    /// <param name="e"> e in this case is the tap position </param> 
    private void OnImgTap(object sender, TappedEventArgs e)
    {
        belowImageLayout.IsVisible = true;

        //bitmap and image have diff dimension/scales so have to convert pixels selected to valid pixels in bitmap
        bMapScaleX = (bitmap.Width / DisplayPhoto.DesiredSize.Width);
        bMapScaleY = (bitmap.Height / DisplayPhoto.DesiredSize.Height);
        //Also important to note that you want to get the DesiredSize.Width and Height in case you change image height in XAML

        //Grab point where user clicked/tapped on image and get specific x and y coords
        Point? p = e.GetPosition((View)sender);

        // Two finger scroll crashes here every time for some reason.
        // trying to keep it from crashing with the clamp
        int x = Math.Clamp(Convert.ToInt32(p.Value.X * bMapScaleX), -2147483646, 2147483646); 
        int y = Math.Clamp(Convert.ToInt32(p.Value.Y * bMapScaleY), -2147483646, 2147483646);

        //Grab specific pixel color from image/bitmapcoords
        SKColor pixelCol = bitmap.GetPixel(x, y);

        //Create Color object from pixel color
        Color holdCol = new Color(pixelCol.Red, pixelCol.Green, pixelCol.Blue, pixelCol.Alpha);
        double red = holdCol.Red * RGB_MAX_VALUE;
        double green = holdCol.Green * RGB_MAX_VALUE;
        double blue = holdCol.Blue * RGB_MAX_VALUE;
        Bgr bgrHoldColor = new Bgr(blue, green, red);
        Hsv[] colorRanges = ComputerVision.ComputeColorRanges(bgrHoldColor);
        Hsv lowerColorLimit = colorRanges[0];
        Hsv upperColorLimit = colorRanges[1];

        // set BoxView colors according to tapped pixel
        tappedColorBox.Color = holdCol;
        lowerBoundaryBox.Color = ComputerVision.HsvToColor(lowerColorLimit);
        upperBoundaryBox.Color = ComputerVision.HsvToColor(upperColorLimit);

        // set slider values to represent color range
        lowerRedSlider.Value = lowerBoundaryBox.Color.Red * RGB_MAX_VALUE;
        upperRedSlider.Value = upperBoundaryBox.Color.Red * RGB_MAX_VALUE;
        lowerGreenSlider.Value = lowerBoundaryBox.Color.Green * RGB_MAX_VALUE;
        upperGreenSlider.Value = upperBoundaryBox.Color.Green * RGB_MAX_VALUE;
        lowerBlueSlider.Value = lowerBoundaryBox.Color.Blue * RGB_MAX_VALUE;
        upperBlueSlider.Value = upperBoundaryBox.Color.Blue * RGB_MAX_VALUE;

        // set properties to initial values
        UpdateProperties();

        PerformCVOperations(lowerColorLimit, upperColorLimit);
    }

    /// <summary>
    /// Replaces the current photo object with a marked-up version with rectangles around objects within the given
    /// Hsv range
    /// </summary>
    /// <param name="lowerColorLimit">Hsv value representing the lower bound of the color range</param>
    /// <param name="upperColorLimit">Hsv value representing the upper bound of the color range</param>
    public static void PerformCVOperations(Hsv lowerColorLimit, Hsv upperColorLimit)
    {
        imageDecodeStream = null;
        if (photo == null)
        {
            return;
        }

        string inputFilePath = photo.FullPath;

        if (string.IsNullOrEmpty(inputFilePath))
        {
            return;
        }

        string base64OutputString = ComputerVision.PerformAnalysis(inputFilePath, lowerColorLimit, upperColorLimit);
        var imageBytes = Convert.FromBase64String(base64OutputString);
        imageDecodeStream = new(imageBytes);
        DisplayPhotoImage.Source = ImageSource.FromStream(() => imageDecodeStream);

        // this if block addresses a bug that causes the image to not be displayed after the
        // first tap of the image. In that case, we set the image's source a second time.
        if (firstImageTap)
        {
            MemoryStream secondaryMemoryStream = new(imageBytes);
            DisplayPhotoImage.Source = ImageSource.FromStream(() => secondaryMemoryStream);
            firstImageTap = false;
        }


    }

    /// <summary>
    /// Executes when one of the sliders on the CameraPage is finished being dragged and updates the internal
    /// values for the lower and upper hue, saturation and value.
    /// </summary>
    /// <param name="sender">The object from which the event originated</param>
    /// <param name="e">The EventArgs for the event</param>
    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        UpdateProperties();
        UpdateBoxViewsFromSliders();
    }

    /// <summary>
    /// Executes when the refresh button is clicked. Updates the image based on the values of the
    /// sliders on the CameraPage
    /// </summary>
    /// <param name = "sender" > The object from which the event originated</param>
    /// <param name="e">The EventArgs for the event</param>
    private void RefreshClicked(object sender, EventArgs e)
    {
        refreshBtn.IsEnabled = false;
        UpdateImage();
        Thread.Sleep(5000);
        refreshBtn.IsEnabled = true;
    }

    /// <summary>
    /// Updates the color of the BoxViews from the values of the sliders on the CameraPage
    /// </summary>
    private void UpdateBoxViewsFromSliders()
    {
        lowerBoundaryBox.Color = Color.FromRgb((int)lowerRedSlider.Value, (int)lowerGreenSlider.Value, (int)lowerBlueSlider.Value);
        upperBoundaryBox.Color = Color.FromRgb((int)upperRedSlider.Value, (int)upperGreenSlider.Value, (int)upperBlueSlider.Value);
        tappedColorBox.Color = CalculateAverageColor(lowerBoundaryBox.Color, upperBoundaryBox.Color);
    }

    /// <summary>
    /// Computes the average color based on the two given Colors.
    /// </summary>
    /// <param name="color1">One of two color inputs.</param>
    /// <param name="color2">One of two color inputs.</param>
    /// <returns>An average of the two given Color objects.</returns>
    private Color CalculateAverageColor(Color color1, Color color2)
    {
        return Color.FromRgb(Math.Sqrt((Math.Pow(color1.Red, 2) + Math.Pow(color2.Red, 2)/2)),
                             Math.Sqrt((Math.Pow(color1.Green, 2) + Math.Pow(color2.Green, 2) / 2)),
                             Math.Sqrt((Math.Pow(color1.Blue, 2) + Math.Pow(color2.Blue, 2) / 2)));
    }

    /// <summary>
    /// Updates the rectangles on the image from the values of the sliders on the CameraPage
    /// </summary>
    private void UpdateImage()
    {
        Hsv lowerHsv = ComputerVision.BgrToHsv(new Bgr(LowerBlue, LowerGreen, LowerRed));
        Hsv upperHsv = ComputerVision.BgrToHsv(new Bgr(upperBlue, UpperGreen, UpperRed));
        CameraPage.PerformCVOperations(lowerHsv, upperHsv);
    }

    /// <summary>
    /// Updates the RGB properties based on the values of the sliders.
    /// </summary>
    private void UpdateProperties()
    {
        LowerRed = (int)lowerRedSlider.Value;
        UpperRed = (int)upperRedSlider.Value;
        LowerGreen = (int)lowerGreenSlider.Value;
        UpperGreen = (int)upperGreenSlider.Value;
        LowerBlue = (int)lowerBlueSlider.Value;
        UpperBlue = (int)upperBlueSlider.Value;
    }

    public int LowerRed
    {
        set
        {
            if (lowerRed != value)
            {
                lowerRed = value;
            }
        }
        get { return lowerRed; }
    }

    public int UpperRed
    {
        set
        {
            if (upperRed != value)
            {
                upperRed = value;
            }
        }
        get { return upperRed; }
    }

    public int LowerGreen
    {
        get { return lowerGreen; }
        set
        {
            if (lowerGreen != value)
            {
                lowerGreen = value;
            }
        }
    }

    public int UpperGreen
    {
        get { return upperGreen; }
        set
        {
            if (upperGreen != value)
            {
                upperGreen = value;
            }
        }
    }

    public int LowerBlue
    {
        get { return lowerBlue; }
        set
        {
            if (lowerBlue != value)
            {
                lowerBlue = value;
            }
        }
    }

    public int UpperBlue
    {
        get { return upperBlue; }
        set
        {
            if (upperBlue != value)
            {
                upperBlue = value;
            }
        }
    }

    /// <summary>
    /// Handles the math for zoom feature. Deals with change in x and y
    /// keeps target coordinates and handles content translation.
    /// </summary>
    /// <param name="sender"> Object responsible for the event.</param>
    /// <param name="e">Eventargs for this event.</param>
    void PinchUpdated(System.Object sender, Microsoft.Maui.Controls.PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            // Store the current scale factor applied to the wrapped user interface element,
            // and zero the components for the center point of the translate transform.
            startScale = Content.Scale;
            Content.AnchorX = 0;
            Content.AnchorY = 0;
        }
        if (e.Status == GestureStatus.Running)
        {
            // Calculate the scale factor to be applied.
            currentScale += (e.Scale - 1) * startScale;
            currentScale = Math.Max(1, currentScale);

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the X pixel coordinate.
            double renderedX = Content.X + xOffset;
            double deltaX = renderedX / Width;
            double deltaWidth = Width / (Content.Width * startScale);
            double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

            // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
            // so get the Y pixel coordinate.
            double renderedY = Content.Y + yOffset;
            double deltaY = renderedY / Height;
            double deltaHeight = Height / (Content.Height * startScale);
            double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

            // Calculate the transformed element pixel coordinates.
            double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
            double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

            // Apply translation based on the change in origin.
            Content.TranslationX = Math.Clamp(targetX, -Content.Width * (currentScale - 1), 0);
            Content.TranslationY = Math.Clamp(targetY, -Content.Height * (currentScale - 1), 0);

            // Apply scale factor
            Content.Scale = currentScale;
        }
        if (e.Status == GestureStatus.Completed)
        {
            // Store the translation delta's of the wrapped user interface element.
            xOffset = Content.TranslationX;
            yOffset = Content.TranslationY;
        }
    }
}
