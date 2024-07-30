using Android.Media;
using System.Collections.ObjectModel;
using Emgu.CV;
using Emgu.CV.CvEnum;
using static Android.Icu.Text.UnicodeSet;
using SQLite;

namespace climb_higher;

public partial class LoginPage : ContentPage
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

    GreetingPage greetingPage = new GreetingPage("user"); //Now username input, so we do "user".
    /// <summary>
    /// LoginPage main page.
    /// </summary>
    public LoginPage()
    {
        InitializeComponent();
        CreateConnection();
        Emgu.CV.Platform.Maui.MauiInvoke.Init();
        loginPageLabel.Text = "Welcome to\n" +
                              "Climb Higher!";
    }
    /// <summary>
    /// Sends user to the home page. outdated name.
    /// </summary>
    /// <param name="sender">Object responsible for the events.</param>
    /// <param name="e">Eventargs for this event.</param>
    private async void login_Clicked(System.Object sender, System.EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
        //await Navigation.PushAsync(greetingPage, true);
    }

    /// <summary>
    /// Shows the list of most recently logged routes. Lists by descending ID #.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();
        routes = new ObservableCollection<ClimbData>();
        List<ClimbData> bList = (from route in conn.Table<ClimbData>().ToList()
                                 orderby route.Id descending
                                 select route).ToList();
        bList.ToList().ForEach(routes.Add);
        recentClimbsLV.ItemsSource = routes;
    }
    /// <summary>
    /// If a user selects one of the routes from the list it sends them to the
    /// routeFocusView Page so they can edit/delete the record.
    /// </summary>
    /// <param name="sender">Object responsible for the event.</param>
    /// <param name="e">Eventargs for this event.</param>
    async void recentClimbs_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        RouteFocusView routeFocusView = new RouteFocusView((ClimbData)recentClimbsLV.SelectedItem);
        await Navigation.PushAsync(routeFocusView, true);
    }
}
