using SQLite;
using System.Collections.ObjectModel;
namespace climb_higher;

/// <summary>
/// StatisticsPage represents the page when user taps statistics in nav bar
/// </summary>
public partial class StatisticsPage : ContentPage
{
    // Creating ObservableCollections to hold best/worst boulder times
    public ObservableCollection<String> boulderBest { get; set; }
    public ObservableCollection<String> boulderWorst { get; set; }

    // Creating ObservableCollections to hold best/worst top rope times
    public ObservableCollection<String> topropeBest { get; set; }
    public ObservableCollection<String> topropeWorst { get; set; }

    // Creating ObservableCollections to hold best/worst lead times
    public ObservableCollection<String> leadBest { get; set; }
    public ObservableCollection<String> leadWorst { get; set; }

    SQLiteConnection conn;
    /// <summary>
    /// CreateConnection() creates the connection to the climb data database
    /// </summary>
    public void CreateConnection()
    {
        string libFolder = FileSystem.AppDataDirectory;
        string fname = System.IO.Path.Combine(libFolder, "ClimbData.db3");
        conn = new SQLiteConnection(fname);
        conn.CreateTable<ClimbData>();
    }

    public string climbType { get; set; }
    climbDataEntryPage entryPage;

    /// <summary>
    /// StatisticsPage() allows this page to be created by another page
    /// </summary>
    public StatisticsPage()
    {
        InitializeComponent();
        CreateConnection();
    }

    /// <summary>
    /// Overriding OnAppearing() to set up entire page with content
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Initializing best/worst time collections for different climb styles
        boulderBest = new ObservableCollection<String>();
        topropeBest = new ObservableCollection<String>();
        leadBest = new ObservableCollection<String>();
        boulderWorst = new ObservableCollection<String>();
        topropeWorst = new ObservableCollection<String>();
        leadWorst = new ObservableCollection<String>();

        // Using Lists to get the best/worst/avg times for boulder routes from database
        List<TimeSpan> boulderBestTimesList = (from route in conn.Table<ClimbData>()
                                               where route.routeType == "Boulder"
                                               select route.BestTime).ToList();
        List<TimeSpan> boulderWorstTimesList = (from route in conn.Table<ClimbData>()
                                                where route.routeType == "Boulder"
                                                select route.WorstTime).ToList();
        List<TimeSpan> boulderAvgTimesList = (from route in conn.Table<ClimbData>()
                                              where route.routeType == "Boulder"
                                              select route.AvgTime).ToList();
        // If we have a 0 TimeSpan then it's either invalid or all times deleted
        // Either way this data is invalid and will mess up the statistics, so we remove it.
        if (boulderBestTimesList.Contains(TimeSpan.Zero))
        {
            boulderBestTimesList.Remove(TimeSpan.Zero);
        }
        if (boulderWorstTimesList.Contains(TimeSpan.Zero))
        {
            boulderWorstTimesList.Remove(TimeSpan.Zero);
        }
        if (boulderAvgTimesList.Contains(TimeSpan.Zero))
        {
            boulderAvgTimesList.Remove(TimeSpan.Zero);
        }

        // If any boulder times exist
        if (boulderBestTimesList.Any())
        {
            // Getting values for best/worst/avg times
            TimeSpan bestTime = boulderBestTimesList.Min();
            TimeSpan worstTime = boulderWorstTimesList.Max();
            double doubleAverageTicks = boulderAvgTimesList.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
            // Chaning text to match times
            bBestClimb.Text = (bestTime.Minutes.ToString("D2") + ":" +
                bestTime.Seconds.ToString("D2") + "." +
                bestTime.Milliseconds.ToString("D3"));
            bWorstClimb.Text = (worstTime.Minutes.ToString("D2") + ":" +
                worstTime.Seconds.ToString("D2") + "." +
                worstTime.Milliseconds.ToString("D3"));

            TimeSpan avgTime = new TimeSpan(longAverageTicks);

            bAvgClimb.Text = (avgTime.Minutes.ToString("D2") + ":" +
                avgTime.Seconds.ToString("D2") + "." +
                avgTime.Milliseconds.ToString("D3"));
            

            bTotalClimbs.Text = (boulderBestTimesList.Count()).ToString();

            // Setting up detailed view for when best boulder is clicked
            bAboutBestClimb.HeightRequest = 50;
            List<String> bBestList = (from route in conn.Table<ClimbData>()
                               where route.routeType == "Boulder"
                               && route.BestTime == bestTime
                               select route.title).ToList();
            bBestList.ToList().ForEach(boulderBest.Add);
            bAboutBestClimb.ItemsSource = boulderBest;

            // Setting up detailed view for when worst boulder is clicked
            bAboutWorstClimb.HeightRequest = 50;
            List<String> bWorstList = (from route in conn.Table<ClimbData>()
                                            where route.routeType == "Boulder"
                                            && route.WorstTime == worstTime
                                            select route.title).ToList();
            bWorstList.ToList().ForEach(boulderWorst.Add);
            bAboutWorstClimb.ItemsSource = boulderWorst;

            bBorder.HeightRequest = 180;

        }
        // In case we have no boulder times
        else
        {
            bTotalClimbs.Text = "0";
            bBestClimb.Text = "No Data";
            bWorstClimb.Text = "No Data";
            bAvgClimb.Text = "No Data";
            bAboutBestClimb.HeightRequest = 0;
            bAboutWorstClimb.HeightRequest = 0;
        }


        // Using Lists to get the best/worst/avg times for lead routes from database
        List<TimeSpan> leadBestTimesList = (from route in conn.Table<ClimbData>()
                                   where route.routeType == "Lead"
                                   select route.BestTime).ToList();

        List<TimeSpan> leadWorstTimesList = (from route in conn.Table<ClimbData>()
                                   where route.routeType == "Lead"
                                   select route.WorstTime).ToList();

        List<TimeSpan> leadAvgTimesList = (from route in conn.Table<ClimbData>()
                                   where route.routeType == "Lead"
                                   select route.AvgTime).ToList();

        // If we have a 0 TimeSpan then it's either invalid or all times deleted
        // Either way this data is invalid and will mess up the statistics, so we remove it.
        if (leadBestTimesList.Contains(TimeSpan.Zero))
        {
            leadBestTimesList.Remove(TimeSpan.Zero);
        }
        if (leadWorstTimesList.Contains(TimeSpan.Zero))
        {
            leadWorstTimesList.Remove(TimeSpan.Zero);
        }
        if (leadAvgTimesList.Contains(TimeSpan.Zero))
        {
            leadAvgTimesList.Remove(TimeSpan.Zero);
        }

        // If any lead times exist
        if (leadBestTimesList.Any())
        {
            // Setting values for best/worst/avg lead times
            TimeSpan bestTime = leadBestTimesList.Min();
            TimeSpan worstTime = leadWorstTimesList.Max();
            double doubleAverageTicks = leadAvgTimesList.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
            // Chaning text values for times
            lBestClimb.Text = (bestTime.Minutes.ToString("D2") + ":" +
                bestTime.Seconds.ToString("D2") + "." +
                bestTime.Milliseconds.ToString("D3"));
            lWorstClimb.Text = (worstTime.Minutes.ToString("D2") + ":" +
                worstTime.Seconds.ToString("D2") + "." +
                worstTime.Milliseconds.ToString("D3"));

            TimeSpan avgTime = new TimeSpan(longAverageTicks);

            lAvgClimb.Text = (avgTime.Minutes.ToString("D2") + ":" +
                avgTime.Seconds.ToString("D2") + "." +
                avgTime.Milliseconds.ToString("D3"));


            lTotalClimbs.Text = (leadBestTimesList.Count()).ToString();

            // Setting up detailed view for when best lead climb is tapped
            lAboutBestClimb.HeightRequest = 50;
            List<String> lBestList = (from route in conn.Table<ClimbData>()
                                           where route.routeType == "Lead"
                                           && route.BestTime == bestTime
                                           select route.title).ToList();
            lBestList.ToList().ForEach(leadBest.Add);
            lAboutBestClimb.ItemsSource = leadBest;

            // Setting up detailed view for when worst lead climb is tapped
            lAboutWorstClimb.HeightRequest = 50;
            List<String> lWorstList = (from route in conn.Table<ClimbData>()
                                            where route.routeType == "Lead"
                                            && route.WorstTime == worstTime
                                            select route.title).ToList();
            lWorstList.ToList().ForEach(leadWorst.Add);
            lAboutWorstClimb.ItemsSource = leadWorst;

            lBorder.HeightRequest = 180;
        } 
        // In case no lead climb times
        else
        {
            lTotalClimbs.Text = "0";
            lBestClimb.Text = "No Data";
            lWorstClimb.Text = "No Data";
            lAvgClimb.Text = "No Data";
            lAboutBestClimb.HeightRequest = 0;
            lAboutWorstClimb.HeightRequest = 0;
        }

        // Using Lists to get the best/worst/avg times for toprope routes from database
        List<TimeSpan> topRopeBestTimesList= (from route in conn.Table<ClimbData>()
                                      where route.routeType == "TopRope"
                                      select route.BestTime).ToList();

        List<TimeSpan> topRopeWorstTimesList = (from route in conn.Table<ClimbData>()
                                      where route.routeType == "TopRope"
                                      select route.WorstTime).ToList();

        List<TimeSpan> topRopeAvgTimesList = (from route in conn.Table<ClimbData>()
                                      where route.routeType == "TopRope"
                                      select route.AvgTime).ToList();

        // If we have a 0 TimeSpan then it's either invalid or all times deleted
        // Either way this data is invalid and will mess up the statistics, so we remove it.
        if (topRopeBestTimesList.Contains(TimeSpan.Zero))
        {
            topRopeBestTimesList.Remove(TimeSpan.Zero);
        }
        if (topRopeWorstTimesList.Contains(TimeSpan.Zero))
        {
            topRopeWorstTimesList.Remove(TimeSpan.Zero);
        }
        if (topRopeAvgTimesList.Contains(TimeSpan.Zero))
        {
            topRopeAvgTimesList.Remove(TimeSpan.Zero);
        }

        // If any toprope times exist
        if (topRopeBestTimesList.Any())
        {
            // Setting values for best/worst/avg toprope times
            TimeSpan bestTime = topRopeBestTimesList.Min();
            TimeSpan worstTime = topRopeWorstTimesList.Max();
            double doubleAverageTicks = topRopeAvgTimesList.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);

            // Changing text for those times
            trBestClimb.Text = (bestTime.Minutes.ToString("D2") + ":" +
                bestTime.Seconds.ToString("D2") + "." +
                bestTime.Milliseconds.ToString("D3"));

            trWorstClimb.Text = (worstTime.Minutes.ToString("D2") + ":" +
                worstTime.Seconds.ToString("D2") + "." +
                worstTime.Milliseconds.ToString("D3"));

            TimeSpan avgTime = new TimeSpan(longAverageTicks);

            trAvgClimb.Text = (avgTime.Minutes.ToString("D2") + ":" +
                avgTime.Seconds.ToString("D2") + "." +
                avgTime.Milliseconds.ToString("D3"));


            trTotalClimbs.Text = (topRopeBestTimesList.Count()).ToString();

            // Setting up detailed view for when best toprope is tapped
            trAboutBestClimb.HeightRequest = 50;
            List<String> trBestList = (from route in conn.Table<ClimbData>()
                                           where route.routeType == "TopRope"
                                           && route.BestTime == bestTime
                                           select route.title).ToList();
            trBestList.ToList().ForEach(topropeBest.Add);
            trAboutBestClimb.ItemsSource = topropeBest;

            // Setting up detailed view for when worst toprope is tapped
            trAboutWorstClimb.HeightRequest = 50;
            List<String> trWorstList = (from route in conn.Table<ClimbData>()
                                            where route.routeType == "TopRope"
                                            && route.WorstTime == worstTime
                                            select route.title).ToList();
            trWorstList.ToList().ForEach(topropeWorst.Add);
            trAboutWorstClimb.ItemsSource = topropeWorst;

            trBorder.HeightRequest = 180;
        } 
        // In case no toprope times exist
        else
        {
            trTotalClimbs.Text = "0";
            trBestClimb.Text = "No Data";
            trWorstClimb.Text = "No Data";
            trAvgClimb.Text = "No Data";
            trAboutBestClimb.HeightRequest = 0;
            trAboutWorstClimb.HeightRequest = 0;
        }
    }

    private async void getMoreInformation(object sender, SelectedItemChangedEventArgs e)
    {
        if(e.SelectedItem != null)
        {
            string routeName = (e.SelectedItem).ToString();
            ClimbData tmp =  (from route in conn.Table<ClimbData>()
                where route.title == routeName
                select route).FirstOrDefault();
            string inOrOut = "";

            if (tmp.isIndoor)
            {
                inOrOut = "Inside";
            } else
            {
                inOrOut = "Outside";
            }

            string information = "Grade: " + tmp.grade + "\nTries: " + tmp.tries + 
                "\nColor: " + tmp.color + "\nWall Type: " + tmp.routeType + 
                "\nInside/Outside: " + inOrOut;

            if (tmp.notes != null)
            {
                information += "\nNotes: " + tmp.notes;
            }

            await DisplayAlert("Information about " + routeName, information, "Exit");
        }
    }
}