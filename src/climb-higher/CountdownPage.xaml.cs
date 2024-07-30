namespace climb_higher;

/// <summary>
/// CountdownPage represents the page that counts down
/// </summary>
public partial class CountdownPage : ContentPage
{

    IDispatcherTimer timer;
    /// <summary>
    /// CountdownPage() allows other pages to open this page
    /// </summary>
    /// <param name="cntDwn"></param>
	public CountdownPage(int cntDwn)
	{
        // We take in an int and use that as seconds to count down
        int secs = cntDwn;
		InitializeComponent();

        // Set up a timer to count down
        countLab.Text = cntDwn.ToString();
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(1000);
        timer.Tick += async (s, e) =>
        {
            secs--;
            countLab.Text = secs.ToString();

            // Once countdown reaches 0 we tell user to GO!, to start climbing
            if (secs <= 0)
            {
                BackgroundColor = Colors.DeepSkyBlue;
                countLab.Text = "GO!";
                await Task.Delay(800);
                //goBack sends user to stopwatch page and starts that timer
                goBack();
            }
        };

        timer.Start();
    }

    /// <summary>
    /// goBack() pushes the user back to the StopwatchPage
    /// </summary>
    private async void goBack()
    {
        await Navigation.PopAsync();
    }
}