using SQLite;

namespace climb_higher;

public partial class climbDataEntryPage : ContentPage
{
    public string climbType { get; set; }
    SQLiteConnection conn;
    /// <summary>
    /// Function used to connect to the database. This is required in every page
    /// that makes any use of the database.
    /// </summary>
    public void CreateConnection()
    {
        string libFolder = FileSystem.AppDataDirectory;
        string fname = System.IO.Path.Combine(libFolder, "ClimbData.db3");
        conn = new SQLiteConnection(fname);
        conn.CreateTable<ClimbData>();
    }
    /// <summary>
    /// climbDataEntryPage main function.
    /// </summary>
    /// <param name="climbType">Used to tell which button the user cliced on the
    /// TrainingPage</param>
    public climbDataEntryPage(string climbType)
	{
        this.climbType = climbType;
		InitializeComponent();
        CreateConnection();
	}
    /// <summary>
    /// Event handler for the "submit" button. This will save the new record
    /// to the database and ensure that the user doesn't input invalid information.
    /// </summary>
    /// <param name="sender"> Object responsible for the event.</param>
    /// <param name="e"Args for this event.</param>
    async void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        int triesInt = 0;
        bool isError = false;
        string gradeStr = grade.Text, walltypeStr = walltype.Text, colorStr = color.Text,
            titleStr = title.Text, triesStr = tries.Text, minsStr = entryTimeMins.Text,
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

        if (String.IsNullOrEmpty(gradeStr)
            && String.IsNullOrEmpty(walltypeStr) && String.IsNullOrEmpty(colorStr)
            && String.IsNullOrEmpty(titleStr) && String.IsNullOrEmpty(triesStr))
        {
            isError = true;
            await DisplayAlert("Entry Error", "Please fill out form before submission.", "OK");
        }
        // 525960 Minutes = Approximately one year, and I'm assuming nobody will take more than a year to start and finish a climb
        else if ((mins > 525960 || mins < 0) ||
            (secs > 59 || secs < 0) ||
            (millisecs > 999 || millisecs < 0) ||
            (mins == 0 && secs == 0 && millisecs == 0))
        {
            isError = true;
            await DisplayAlert("Entry Error", "Invalid Time Entered", "OK");
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
            TimeSpan time = new TimeSpan(0, 0, mins, secs, millisecs);

            ClimbData climb = new ClimbData
            {
                grade = gradeStr,
                tries = triesInt,
                walltype = walltypeStr,
                color = colorStr,
                notes = notes.Text,
                title = titleStr,
                isIndoor = indoor.IsEnabled,
                routeType = climbType,
                timeLength = time,
                BestTime = time,
                AvgTime = time,
                WorstTime = time,
                stringOfTimes = time.Minutes.ToString() + ":"  + time.Seconds.ToString() + "." + time.Milliseconds.ToString()

                
            };
            conn.Insert(climb);
            await Navigation.PopAsync();
        }
    }

}
