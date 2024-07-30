namespace climb_higher;
using Microsoft.Maui.Controls.Hosting;

public partial class GreetingPage : ContentPage
{
    public string userName { get; set; } // Kind of superfluous now. Since login feature is out of scope.
    /// <summary>
    /// GreetingPage main page.
    /// </summary>
    /// <param name="userName">Can be used for implementing userName/login feature
    /// in the future. But right now it's kind of useless.</param>
    public GreetingPage(string userName)
    {
        InitializeComponent();
        label.Text = "Welcome, " + userName;
    }
    /// <summary>
    /// Takes user to Camera Page.
    /// </summary>
    /// <param name="sender">Object responsible for the event.</param>
    /// <param name="e">Eventargs for this event.</param>
    private async void cameraButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CameraPage(), true);
    }

    /// <summary>
    /// Takes user to Training Page.
    /// </summary>
    /// <param name="sender">Object responsible for the event.</param>
    /// <param name="e">Eventargs for this event.</param>
    private async void trainingButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new TrainingPage(), true);
    }

    /// <summary>
    /// Takes user to Stopwatch Page.
    /// </summary>
    /// <param name="sender">Object responsible for the event.</param>
    /// <param name="e">Eventargs for this event.</param>
    private async void stopwatchButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new StopwatchPage(), true);
    }

    /// <summary>
    /// Takes user to Settings Page.
    /// </summary>
    /// <param name="sender">Object responsible for the event.</param>
    /// <param name="e">Eventargs for this event.</param>
    private async void settingsButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage(), true);
    }
}
