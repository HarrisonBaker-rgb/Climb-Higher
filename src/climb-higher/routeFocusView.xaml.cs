using SQLite;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Numerics;

namespace climb_higher;

/// <summary>
/// RouteFocusView represents the page that comes up when going to edit a climb
/// </summary>
public partial class RouteFocusView : ContentPage
{
    public ClimbData data;
    public string climbType { get; set; }
    public string climb { get; set; }
    SQLiteConnection conn;
    private ObservableCollection<Time> timeCollection = new ObservableCollection<Time>();

    public void CreateConnection()
    {
        string libFolder = FileSystem.AppDataDirectory;
        string fname = System.IO.Path.Combine(libFolder, "ClimbData.db3");
        conn = new SQLiteConnection(fname);
        conn.CreateTable<ClimbData>();
    }
    public RouteFocusView(ClimbData data)
    {

        this.climbType = data.routeType;
        this.climb = climb;
        InitializeComponent();
        CreateConnection();
        this.data = data;
        grade.Text = data.grade;
        tries.Text = data.tries + "";
        color.Text = data.color;
        notes.Text = data.notes;
        walltype.Text = data.walltype;
        indoor.IsToggled = data.isIndoor;
        title.Text = data.title;
        entryTimeMins.Placeholder = "0";
        entryTimeSecs.Placeholder = "0";
        entryTimeMillisecs.Placeholder = "0";

        updateListView();
        timeLV.ItemsSource = timeCollection;

    }

    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {

        int triesInt = 0;
        bool isError = false;
        string gradeStr = grade.Text, walltypeStr = walltype.Text, colorStr = color.Text,
            titleStr = title.Text, triesStr = tries.Text, minsStr = entryTimeMins.Text,
            secsStr = entryTimeSecs.Text, millisecsStr = entryTimeMillisecs.Text;

        if (String.IsNullOrEmpty(gradeStr)
            && String.IsNullOrEmpty(walltypeStr) && String.IsNullOrEmpty(colorStr)
            && String.IsNullOrEmpty(titleStr) && String.IsNullOrEmpty(triesStr))
        {
            isError = true;
            await DisplayAlert("Entry Error", "Please fill out form before submission.", "OK");
        }
 
        else if (String.IsNullOrEmpty(gradeStr) || String.IsNullOrEmpty(walltypeStr)
            || String.IsNullOrEmpty(colorStr) || String.IsNullOrEmpty(titleStr))
        {
            isError = true;
            await DisplayAlert("Entry Error", "Invalid Grade, Walltype, Color, or Title.", "OK");
        }
        else if (String.IsNullOrEmpty(triesStr) || !int.TryParse(triesStr, out triesInt) || triesInt < 0)
        {
            isError = true;
            await DisplayAlert("Entry Error", "Invalid Tries.", "OK");
        }

        if (!isError)
        {

            data.grade = gradeStr;
            data.tries = triesInt;
            data.walltype = walltypeStr;
            data.color = colorStr;
            data.notes = notes.Text;
            data.title = titleStr;
            data.isIndoor = indoor.IsToggled;
            data.routeType = climbType;
            TimeSpan bTime = data.findBestTime();
            data.BestTime = bTime;
            TimeSpan avgTime = data.findAvgTime();
            data.AvgTime = avgTime;
            TimeSpan wTime = data.findWorstTime();
            data.WorstTime = wTime;
            
            conn.Update(data);
            await Navigation.PopAsync();
        }
    }

    /// <summary>
    /// addTimeBtn_Clicked() allows the user to add a time to the climb
    /// </summary>
    /// <param name="sender"> Object from add time button </param>
    /// <param name="e"> Event from add time button </param>
    async void addTimeBtn_Clicked(object sender, EventArgs e)
    {
        // Start by doing a lot of checks to ensure numbers entered are valid
        bool isError = false;
        String minsStr = entryTimeMins.Text,
            secsStr = entryTimeSecs.Text, millisecsStr = entryTimeMillisecs.Text;

        if (String.IsNullOrEmpty(minsStr)) { minsStr = "0"; }
        if (String.IsNullOrEmpty(secsStr)) { secsStr = "0"; }
        if (String.IsNullOrEmpty(millisecsStr)) { millisecsStr = "0"; }

        async void checkDec(String str)
        {
            foreach (char c in str)
            {
                if (c == '.')
                {
                    isError = true;
                    await DisplayAlert("Entry Error", "Please do not use decimals for times.", "OK");
                }
            }
        }

        checkDec(minsStr); checkDec(secsStr); checkDec(millisecsStr);

        int mins = 0, secs = 0, millisecs = 0;
        if (!isError)
        {
            mins = Convert.ToInt32(minsStr);
            secs = Convert.ToInt32(secsStr);
            millisecs = Convert.ToInt32(millisecsStr);
        }

        if ((mins > 525960 || mins < 0) ||
            (secs > 59 || secs < 0) ||
            (millisecs > 999 || millisecs < 0) ||
            (mins == 0 && secs == 0 && millisecs == 0))
        {
            isError = true;
            await DisplayAlert("Entry Error", "Invalid Time Entered", "OK");
        }

        if (!isError) 
        {
            // If no error with time, then we turn it into a TimeSpan and enter
            // it into the climb database as part of the stringOfTimes of that climb
            TimeSpan time = new TimeSpan(0, 0, mins, secs, millisecs);
            if (String.IsNullOrEmpty(data.stringOfTimes))
            {
                data.stringOfTimes = time.Minutes.ToString() + ":" + time.Seconds.ToString() + "." + time.Milliseconds.ToString();
            }
            else
            {
                data.stringOfTimes += "," + time.Minutes.ToString() + ":" + time.Seconds.ToString() + "." + time.Milliseconds.ToString();
            }
            // increasing number of tries by one if they add a time, this may be
            // more of an annoyance to some users and could be removed
            data.tries++;
            tries.Text = data.tries.ToString();
            conn.Update(data);
            data.BestTime = data.findBestTime();
            data.WorstTime = data.findWorstTime();
            data.AvgTime = data.findAvgTime();
            conn.Update(data);
            await DisplayAlert("Entry Status", "Successful Entry", "OK");
            updateListView();
        }

    }

    /// <summary>
    /// viewTimesBtn_Clicked() allows the user to hide and reveal the times
    /// within a climb, as if there are a lot it can be a lot to scroll through
    /// </summary>
    /// <param name="sender"> Object from view times button </param>
    /// <param name="e">Event from view times button </param>
    private void viewTimesBtn_Clicked(object sender, EventArgs e)
    {
        updateListView();
        timeLV.IsVisible = !timeLV.IsVisible;
        if (timeLV.IsVisible)
        {
            viewTimesBtn.Text = "Hide Times";
        } 
        else
        {
            viewTimesBtn.Text = "View Times";
        }
        
    }

    /// <summary>
    /// timeLV_ItemTapped() allows the user to tap on a time in the listview
    /// and then edit that time in the EditStopwatchTime page
    /// </summary>
    /// <param name="sender"> Object (listview) from listview item click</param>
    /// <param name="e"> ItemTapped (time) from listview item click</param>
    private void timeLV_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        Navigation.PushModalAsync(new EditStopwatchTime(e.Item as Time, "routeFocusView", data), true);
        updateListView();
    }

    /// <summary>
    /// updateListView() is a method to update the listview of times
    /// </summary>
    private void updateListView()
    {
        timeCollection.Clear();
        if (!String.IsNullOrEmpty(data.stringOfTimes))
        {

            String[] listOfTimes = data.stringOfTimes.Split(',');
            foreach (String s in listOfTimes)
            {
                // s format example: 02:14.3450000
                String[] listTimeSplit = (s.Split(':')); // {02, 14.3450000}
                String[] splitSecsMillisecs = (listTimeSplit[1].Split(".")); // {14, 3450000}

                timeCollection.Add(new Time
                {
                    Mins = int.Parse(listTimeSplit[0]),
                    Secs = int.Parse(splitSecsMillisecs[0]),
                    Millisecs = int.Parse(splitSecsMillisecs[1])
                });
                // Divide millisecs by 10,000, since the TimeSpan property uses
                // 7 digits for millisecs while we use 3 digits.
            }


        }
        timeLV.ItemsSource = timeCollection;
    }

    /// <summary>
    /// Overriding OnAppearing() to make sure when page appears, everything is
    /// displayed correctly
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        updateListView();
        tries.Text = data.tries.ToString();
    }

    /// <summary>
    /// DelBtn_Clicked() is the method behind the delete button which allows
    /// the user to delete the selected climb from the database.
    /// May be important to note that the database IDs for climbs are used,
    /// so when a new climb is made, the deleted ID is not reused, it keeps
    /// incrementing, which could mean very large IDs after a while.
    /// </summary>
    /// <param name="sender"> Object from Delete button</param>
    /// <param name="e"> Event from Delete Button</param>
    private async void DelBtn_Clicked(object sender, EventArgs e)
    {
        conn.Delete(data);
        await Navigation.PopAsync();
    }
}
