using System.Diagnostics;
using SQLite;

namespace climb_higher;

/// <summary>
/// StopwatchPage represents that page that opens we the timer icon in the
/// navigation bar is tapped. Allows user to time themself.
/// </summary>
public partial class StopwatchPage : ContentPage 
{
    SQLiteConnection conn;
    /// <summary>
    /// CreateConn() creates the connection to the database of times
    /// </summary>
    public void CreateConn()
    {
        string folder = FileSystem.AppDataDirectory;
        string file = System.IO.Path.Combine(folder, "stopwatchTimes.db");
        conn = new SQLiteConnection(file);
        conn.CreateTable<Time>();
    }

    IDispatcherTimer timer;
    long totalMillisecs = 0;
    long millisecs = 0;
    long secs = 0;
    long mins = 0;
    bool started = false;
    int cntDwn = 0;

    Stopwatch sw;

    /// <summary>
    /// StopwatchPage() allows us to create the page from other pages
    /// </summary>
    public StopwatchPage()
	{
        InitializeComponent();

        CreateConn();

        // Filling listview with times from database
        timeLV.ItemsSource = conn.Table<Time>().ToList();

        // Using two timers here, one to track the total time and
        // another to track minutes
        // There is likely a way to track minutes with total time
        // and this could likely be written better
        sw = new Stopwatch();

        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(1);
        timer.Tick += (s, e) =>
        {
            totalMillisecs = sw.ElapsedMilliseconds;
            millisecsLab.Text = millisecs.ToString("D3");
            secsLab.Text = secs.ToString("D2") + ".";
            minsLab.Text = mins.ToString("D2") + ":";

            millisecs = totalMillisecs % 1000;
            secs = totalMillisecs / 1000;

            if (secs == 60)
            {
                secs = 0;
                mins++;
                sw.Restart();
            }
            
        };


    }


    /// <summary>
    /// Start_Clicked() starts the stopwatch, unless we have a countdown
    /// in which case that occurs first, then this method follows.
    /// </summary>
    /// <param name="sender"> arbitrary object from button </param>
    /// <param name="e"> event from button </param>
    private async void Start_Clicked(object sender, EventArgs e)
    {

        // Since the same button for starting is also used for stopping and
        // restarting we check to see if we have already started it before
        if (!started)
        {
            // Doing some checks for countdown
            if (CntdwnPicker.SelectedItem == null)
            {
                cntDwn = 0;
            } else
            {
                cntDwn = Convert.ToInt32(CntdwnPicker.SelectedItem.ToString());
            }
            
            if (cntDwn != 0)
            {
                // Calls the countdown page, the false means no animation to the page
                await Navigation.PushAsync(new CountdownPage(cntDwn), false);
            }
            await Task.Delay(cntDwn * 1000);

            Start.Text = "Stop";
            millisecs = 0;
            secs = 0;
            mins = 0;
            timer.Start();
            sw.Start();
        } else
        {
            Start.Text = "Restart";
            stop();
        }

        started = !started;
    }

    /// <summary>
    /// stop() is called when we want to stop the stopwatch from counting.
    /// </summary>
    private void stop()
    {
        //We stop the stopwatch and reset it, we stop the timer, then we just slightly alter the timer text
        //-- at the top of the screen so that it matches the time that pops up in the listview when we add a new time to times list
        sw.Stop();
        sw.Reset();
        timer.Stop();
        // Set the texts so the displays correctly match
        millisecsLab.Text = millisecs.ToString("D3");
        secsLab.Text = secs.ToString("D2") + ".";
        minsLab.Text = mins.ToString("D2") + ":";
        // Creating a time based on the stopwatch and enters it into database and listview
        Time t = new Time {
            Mins = Convert.ToInt32(mins),
            Secs = Convert.ToInt32(secs),
            Millisecs = Convert.ToInt32(millisecs),
        };
        conn.Insert(t);
        timeLV.ItemsSource = conn.Table<Time>().ToList();
    }

    /// <summary>
    /// Overriding the OnAppearing() method to also update listview
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        timeLV.ItemsSource = conn.Table<Time>().ToList();
    }

    /// <summary>
    /// When a time in the listview it tapped we open the page to edit times
    /// </summary>
    /// <param name="sender"> Object that represents anything sent with button </param>
    /// <param name="e"> Event arguments coming from tapped item </param>
    private void timeLV_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Navigation.PushAsync(new EditStopwatchTime(e.Item as Time, "StopwatchPage"), true);
    }

    /// <summary>
    /// Changes "Countdown" to "Seconds" to give user a better idea of countdown
    /// </summary>
    /// <param name="sender"> Object sent with button click </param>
    /// <param name="e"> Event arguments comi g from button </param>
    private void CntdwnPicker_Focused(object sender, FocusEventArgs e)
    {
        Task.Delay(3000);
        CntdwnPicker.Title = "Seconds";
    }

}