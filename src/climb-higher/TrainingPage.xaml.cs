using SQLite;
using System.Collections.ObjectModel;
namespace climb_higher;

public partial class TrainingPage : ContentPage
{
    public ObservableCollection<ClimbData> boulder { get; set; }
    public ObservableCollection<ClimbData> toprope { get; set; }
    public ObservableCollection<ClimbData> lead { get; set; }
    SQLiteConnection conn;
    public void CreateConnection()
    {
        string libFolder = FileSystem.AppDataDirectory;
        string fname = System.IO.Path.Combine(libFolder, "ClimbData.db3");
        conn = new SQLiteConnection(fname);
        conn.CreateTable<ClimbData>();
    }

    public TrainingPage()
	{
		InitializeComponent();
        CreateConnection();
	}
    ClimbPage climbPage;
    protected override void OnAppearing()
    {
        base.OnAppearing();
        boulder = new ObservableCollection<ClimbData>();
        toprope = new ObservableCollection<ClimbData>();
        lead = new ObservableCollection<ClimbData>();

        List<String> nothing = new List<String>();
        List<ClimbData> boulderList = (from route in conn.Table<ClimbData>()
                                       where route.routeType == "Boulder"
                                       orderby route.Id descending
                                       select route).ToList();
        if (boulderList.Take(2).Count() == 2)
        {
            boulder.Add(boulderList[0]);
            boulder.Add(boulderList[1]);
        } else if (boulder.Any())
        {
            boulder.Add(boulderList[0]);
        }
        bRecentClimbs.ItemsSource = boulder;

        List<ClimbData> topropeList = (from route in conn.Table<ClimbData>()
                                    where route.routeType == "TopRope"
                                    orderby route.Id descending
                                    select route).ToList();
        if (topropeList.Take(2).Count() == 2)
        {
            toprope.Add(topropeList[0]);
            toprope.Add(topropeList[1]);
        } else if (topropeList.Any())
        {
            toprope.Add(topropeList[0]);
        }
        trRecentClimbs.ItemsSource = toprope;

        List<ClimbData> leadList = (from route in conn.Table<ClimbData>()
                                 where route.routeType == "Lead"
                                 orderby route.Id descending
                                 select route).ToList();
        if (leadList.Take(2).Count() == 2)
        {
            lead.Add(leadList[0]);
            lead.Add(leadList[1]);
            //lRecentClimbs.ItemsSource = lead;
        } else if (leadList.Any())
        {
            lead.Add(leadList[0]);
            //lRecentClimbs.ItemsSource = lead;
        } else
        {
            lead = null;
        }
        lRecentClimbs.ItemsSource = lead;
    }

    async void topRopeButton_Clicked(System.Object sender, System.EventArgs e)
    {
        climbPage = new ClimbPage("TopRope");
        await Navigation.PushAsync(climbPage,true);
    }

    async void leadButton_Clicked(System.Object sender, System.EventArgs e)
    {
        climbPage = new ClimbPage("Lead");
        await Navigation.PushAsync(climbPage, true);
    }

    async void boulderButton_Clicked(System.Object sender, System.EventArgs e)
    {
        climbPage = new ClimbPage("Boulder");
        await Navigation.PushAsync(climbPage, true);
    }

    async void statisticsButton_Clicked(object sender, EventArgs e)
    {
        StatisticsPage statisticsPage = new StatisticsPage();
        await Navigation.PushAsync(statisticsPage, true);
    }

    async void bRecentClimbs_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        RouteFocusView routeFocusView = new RouteFocusView((ClimbData)bRecentClimbs.SelectedItem);
        await Navigation.PushAsync(routeFocusView, true);
    }

    async void trRecentClimbs_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        RouteFocusView routeFocusView = new RouteFocusView((ClimbData)trRecentClimbs.SelectedItem);
        await Navigation.PushAsync(routeFocusView, true);
    }

    async void lRecentClimbs_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        RouteFocusView routeFocusView = new RouteFocusView((ClimbData)lRecentClimbs.SelectedItem);
        await Navigation.PushAsync(routeFocusView, true);
    }
}
