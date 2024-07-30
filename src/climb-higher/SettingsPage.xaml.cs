namespace climb_higher;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    void YellowBlue_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {

        App.Current.Resources["priColor"] = (Color)(App.Current.Resources["yellowColor"]);
        App.Current.Resources["secColor"] = (Color)(App.Current.Resources["blueColor"]);
        App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightBlueColor"]);
        App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightYellowColor"]);

        App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["blueColor"]);
        App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["yellowColor"]);

        Preferences.Set("ThemePreference", "YellowBlue.xaml");
    }

    void TanTurquoise_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        App.Current.Resources["priColor"] = (Color)(App.Current.Resources["tanColor"]);
        App.Current.Resources["secColor"] = (Color)(App.Current.Resources["turquoiseColor"]);
        App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightTurquoiseColor"]);
        App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightTanColor"]);

        App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["turquoiseColor"]);
        App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["tanColor"]);
        Preferences.Set("ThemePreference", "TanTurquoise.xaml");
    }

    void OrangePurple_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        App.Current.Resources["priColor"] = (Color)(App.Current.Resources["orangeColor"]);
        App.Current.Resources["secColor"] = (Color)(App.Current.Resources["purpleColor"]);
        App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightPurpleColor"]);
        App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightOrangeColor"]);

        App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["purpleColor"]);
        App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["orangeColor"]);
        Preferences.Set("ThemePreference", "OrangePurple.xaml");   
    }

    void GreenPurple_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {

        App.Current.Resources["priColor"] = (Color)(App.Current.Resources["greenColor"]);
        App.Current.Resources["secColor"] = (Color)(App.Current.Resources["purpleColor"]);
        App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightPurpleColor"]);
        App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightGreenColor"]);

        App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["purpleColor"]);
        App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["greenColor"]);
        Preferences.Set("ThemePreference", "GreenPurple.xaml");
    }

    void BlueRed_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        App.Current.Resources["priColor"] = (Color)(App.Current.Resources["blueColor"]);
        App.Current.Resources["secColor"] = (Color)(App.Current.Resources["redColor"]);
        App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightRedColor"]);
        App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightBlueColor"]);

        App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["redColor"]);
        App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["blueColor"]);
        Preferences.Set("ThemePreference", "BlueRed.xaml");

    }

    void BluePink_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        App.Current.Resources["priColor"] = (Color)(App.Current.Resources["blueColor"]);
        App.Current.Resources["secColor"] = (Color)(App.Current.Resources["pinkColor"]);
        App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightPinkColor"]);
        App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightBlueColor"]);

        App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["pinkColor"]);
        App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["blueColor"]);
        Preferences.Set("ThemePreference", "BluePink.xaml");   
    }

    void YellowPink_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        App.Current.Resources["priColor"] = (Color)(App.Current.Resources["yellowColor"]);
        App.Current.Resources["secColor"] = (Color)(App.Current.Resources["pinkColor"]);
        App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightPinkColor"]);
        App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightYellowColor"]);

        App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["pinkColor"]);
        App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["yellowColor"]);
        Preferences.Set("ThemePreference", "YellowPink.xaml");
    }

    void BrownBlue_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        App.Current.Resources["priColor"] = (Color)(App.Current.Resources["brownColor"]);
        App.Current.Resources["secColor"] = (Color)(App.Current.Resources["blueColor"]);
        App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightBlueColor"]);
        App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightBrownColor"]);

        App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["blueColor"]);
        App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["brownColor"]);
        Preferences.Set("ThemePreference", "BrownBlue.xaml");
    }

    void Default_Tapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        App.Current.Resources["priColor"] = (Color)(Color.FromArgb("#74E291"));
        App.Current.Resources["secColor"] = (Color)(Color.FromArgb("#CCF2D1"));
        App.Current.Resources["triColor"] = (Color)(Color.FromArgb("#f1eee8"));
        App.Current.Resources["quadColor"] = (Color)(Color.FromArgb("#f1eee8"));

        App.Current.Resources["tempTri"] = (Color)(Color.FromArgb("#f1eee8"));
        App.Current.Resources["tempQuad"] = (Color)(Color.FromArgb("#000000"));

        Preferences.Set("ThemePreference", "DefaultTheme.xaml");
    }

    private async void PrivacyButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrivacyPage(), true);
    }
}
