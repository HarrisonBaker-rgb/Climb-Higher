using SQLite;

namespace climb_higher;

/// <summary>
/// EditStopwatchTime is a class that represents the page that pops up to edit
/// an instance of a time
/// </summary>
public partial class EditStopwatchTime : ContentPage
{

    // conn is the connection to the SQLite database
    SQLiteConnection conn;
    // t is the time that we are editing
	Time t;
    // prevPage is a string that represents the previous page the user was on
    String prevPage;
    // data is an object the represents the route info which might hold a time
    ClimbData data;

    /// <summary>
    /// CreateConn() sets up the SQLite connection for the database of times
    /// </summary>
    public void CreateConn()
    {
        string folder = FileSystem.AppDataDirectory;
        string file = System.IO.Path.Combine(folder, "stopwatchTimes.db");
        conn = new SQLiteConnection(file);
        conn.CreateTable<Time>();
    }

    /// <summary>
    /// CreateConnClimbData() sets up SQLite connection for database of climbs
    /// </summary>
    public void CreateConnClimbData()
    {
        string libFolder = FileSystem.AppDataDirectory;
        string fname = System.IO.Path.Combine(libFolder, "ClimbData.db3");
        conn = new SQLiteConnection(fname);
        conn.CreateTable<ClimbData>();
    }

    /// <summary>
    /// EditStopWatchTime() allows us to create the page from the previous page
    /// </summary>
    /// <param name="t"> represents the time user might edit </param> 
    /// <param name="prevPage"> represents the previous page user was on </param>
    /// <param name="data"> represents data which holds time user may edit </param>
    public EditStopwatchTime(Time t, String prevPage, ClimbData data = null)
	{
        this.t = t;
        this.prevPage = prevPage;
        this.data = data;

		InitializeComponent();
        // Creating a different connection depending on what user's previous page was
        if (prevPage == "StopwatchPage")
        {
            CreateConn();
        }
        else // (prevPage == "routeFocusView"), if not that, something wrong
        {
            CreateConnClimbData();
        } 

        // Setting up text entries with time to be edited
        entryTimeMins.Text = t.Mins.ToString("D2");
		entryTimeSecs.Text = t.Secs.ToString("D2");
		entryTimeMillisecs.Text = t.Millisecs.ToString("D3");
	}

    /// <summary>
    /// SaveEdit_Clicked() is run when the user clicks SaveEdit button
    /// It saves the new time in place of the old time
    /// </summary>
    /// <param name="sender"> object based on button </param> 
    /// <param name="e"> event data from button click </param> 
    private async void SaveEdit_Clicked(object sender, EventArgs e)
    {
        // Checking if entry numbers are null or empty
        if (String.IsNullOrEmpty(entryTimeMins.Text)) { entryTimeMins.Text = "0"; }
        if (String.IsNullOrEmpty(entryTimeSecs.Text)) { entryTimeSecs.Text = "0"; }
        if (String.IsNullOrEmpty(entryTimeMillisecs.Text)) { entryTimeMillisecs.Text = "0"; }

        bool isError = false;
        string minsStr = entryTimeMins.Text,
            secsStr = entryTimeSecs.Text, millisecsStr = entryTimeMillisecs.Text;

        // Ensure entry numbers are not decimals
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
            mins = Convert.ToInt32(entryTimeMins.Text);
            secs = Convert.ToInt32(entryTimeSecs.Text);
            millisecs = Convert.ToInt32(entryTimeMillisecs.Text);
        }

        // Ensuring entered numbers are valid
        if ((mins > 525960 || mins < 0) ||
            (secs > 59 || secs < 0) ||
            (millisecs > 999 || millisecs < 0) ||
            (mins == 0 && secs == 0 && millisecs == 0))
        {
            isError = true;
            await DisplayAlert("Entry Error", "Invalid Time Entered", "OK");
        }

        //Have to disable and re-enable the text entries to hide the soft keyboard otherwise
        //-- when we go back to the Stopwatch page the keyboard will open the countdown picker
        entryTimeMins.IsEnabled = false;
        entryTimeSecs.IsEnabled = false;
        entryTimeMillisecs.IsEnabled = false;
        entryTimeMins.IsEnabled = true;
        entryTimeSecs.IsEnabled = true;
        entryTimeMillisecs.IsEnabled = true;

        // Do not continue if time error
        if (!isError)
        {
            // Simply editing a time from Stopwatch page
            if (prevPage == "StopwatchPage")
            {
                t.Mins = mins;
                t.Secs = secs;
                t.Millisecs = millisecs;
                conn.Update(t);
            }
            // Editing a climbdata time takes some more effort before updating
            else // (prevPage == "routeFocusView"), if not that, something wrong
            {
                TimeSpan tSpan = t.toTimeSpan();
                String chStr = tSpan.Minutes.ToString()
                    + ":" + tSpan.Seconds.ToString()
                    + "." + tSpan.Milliseconds.ToString();
                String[] arrOfTimes = data.stringOfTimes.Split(',');
                int changeHere = 0;
                bool foundS = false;

                // Multiple times saved in a climb are saved as one long string
                // so we have to convert the times into strings a certain way
                foreach (String s in arrOfTimes)
                {
                    s.Trim();
                    if (chStr == s)
                    {
                        t.Mins = mins;
                        t.Secs = secs;
                        t.Millisecs = millisecs;
                        tSpan = t.toTimeSpan();
                        foundS = true;

                        chStr = tSpan.Minutes.ToString()
                            + ":" + tSpan.Seconds.ToString()
                            + "." + tSpan.Milliseconds.ToString();

                        arrOfTimes[changeHere] = chStr;
                        String strTimes = "";
                        foreach (String time in arrOfTimes)
                        {
                            if (strTimes == "")
                            {
                                strTimes = time;
                            } else
                            {
                                strTimes += "," + time;
                            }
                            
                        }
                        data.stringOfTimes = strTimes;
                        conn.Update(data);
                        data.BestTime = data.findBestTime();
                        data.WorstTime = data.findWorstTime();
                        data.AvgTime = data.findAvgTime();
                        conn.Update(data);

                    }
                    changeHere++;
                }

                if (!foundS)
                {
                    await DisplayAlert("Error", "Time Not Found", "OK");
                }

            }

            // Different ways of going back to previous page, depends on how
            // this page was opened
            if (prevPage == "StopwatchPage")
            {
                await Navigation.PopAsync();
            } else
            {
                await Navigation.PopModalAsync();
            }

        }
    }

    /// <summary>
    /// Delete_Clicked() deletes the time that was selected from the database
    /// </summary>
    /// <param name="sender"> object based on button </param>
    /// <param name="e"> event data from button </param> 
    private async void Delete_Clicked(object sender, EventArgs e)
    {
        // If just deleting from stopwatch, then easy delete
        if (prevPage == "StopwatchPage")
        {
            conn.Delete(t);
            await Navigation.PopAsync();
        }
        // Deleting from a climb involves finding the climb to ensure we
        // are deleting the correct time and we have to delete it from
        // a string within a climb object in a database
        else
        {
            TimeSpan tSpan = t.toTimeSpan();
            String[] arrOfTimes = data.stringOfTimes.Split(',');
            List<String> listOfTimes = arrOfTimes.ToList();

            String rmStr = tSpan.Minutes.ToString()
                + ":" + tSpan.Seconds.ToString()
                + "." + tSpan.Milliseconds.ToString();

            listOfTimes.Remove(rmStr);
            data.stringOfTimes = "";
            int i = 0;
            foreach(String s in listOfTimes)
            {
                if (i == 0)
                {
                    data.stringOfTimes = s;
                }
                else
                {
                    data.stringOfTimes += ',' + s;
                }
                i++;
            }

            if (listOfTimes.Count == 0)
            {
                data.stringOfTimes = null;
            }
            
            data.tries -= 1;
            conn.Update(data);
            data.BestTime = data.findBestTime();
            data.WorstTime = data.findWorstTime();
            data.AvgTime = data.findAvgTime();
            conn.Update(data);
            await Navigation.PopModalAsync();
        }

    }
}