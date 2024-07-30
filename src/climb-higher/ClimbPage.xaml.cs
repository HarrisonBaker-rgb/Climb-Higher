using SQLite;
using System.Collections.ObjectModel;

namespace climb_higher;

public partial class ClimbPage : ContentPage
{
    public ObservableCollection<ClimbData> routes;
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

    public string climbType { get; set; }
    climbDataEntryPage entryPage;

    /// <summary>
    /// ClimbPage main function.
    /// </summary>
    /// <param name="climbType">Determines which list to load. We check this to
    /// know which button the user clicked on TrainingPage</param>
    public ClimbPage(string climbType)
	{
        this.climbType = climbType;
        InitializeComponent();
        CreateConnection();
	}
    /// <summary>
    /// On the page appearing we show a list of climbs of the discipline selected.
    /// We check climbType to see which button the user selected.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        routes = new ObservableCollection<ClimbData>();
        if (climbType == "Boulder")
        {
            theLabel.Text = "Boulder Climbs";
            entryPage = new climbDataEntryPage("Boulder");
            List<ClimbData> bList = (from route in conn.Table<ClimbData>().ToList()
                                            where route.routeType == "Boulder"
                                            select route).ToList();
            bList.ToList().ForEach(routes.Add);
        }
        else if (climbType == "Lead")
        {
            theLabel.Text = "Lead";
            entryPage = new climbDataEntryPage("Lead");
            List<ClimbData> lList = (from route in conn.Table<ClimbData>().ToList()
                                          where route.routeType == "Lead"
                                          select route).ToList();
            lList.ToList().ForEach(routes.Add);
        }
        else if (climbType == "TopRope")
        {
            theLabel.Text = "Top Rope Climbs";
            entryPage = new climbDataEntryPage("TopRope");
            List<ClimbData> trList = (from route in conn.Table<ClimbData>().ToList()
                                          where route.routeType == "TopRope"
                                          select route).ToList();
            trList.ToList().ForEach(routes.Add);
        }
        finishedClimbLV.ItemsSource = routes;
    }
    /// <summary>
    /// Brings the user to the entryPage, where they can enter a new route.
    /// </summary>
    /// <param name="sender">The object responsible for the event.</param>
    /// <param name="e">Event args for this event.</param>
    async void newClimbButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(entryPage, true);
    }

    async void finishedClimbLV_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        RouteFocusView routeFocusView = new RouteFocusView((ClimbData)finishedClimbLV.SelectedItem);
        await Navigation.PushAsync(routeFocusView,true);
    }
}


