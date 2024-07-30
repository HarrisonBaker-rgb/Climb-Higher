//using Xamarin.Forms;

namespace climb_higher
{
    public partial class App : Application
    {
        const string ThemePreferenceKey = "ThemePreference";

        public App()
        {
            InitializeComponent();
            LoadThemePreference();
            MainPage = new AppShell();
        }

        void LoadThemePreference()
        {
            if (Preferences.ContainsKey(ThemePreferenceKey))
            {
                var themePreference = Preferences.Get(ThemePreferenceKey, string.Empty);
                ApplyTheme(themePreference);
            }
        }

        void ApplyTheme(string themePreference)
        {
            if (Application.Current != null && Application.Current.Resources != null)
            {
                switch (themePreference)
                {
                    case "YellowBlue.xaml":
                        ApplyYellowBlueTheme();
                        break;
                    case "TanTurquoise.xaml":
                        ApplyTanTurquoiseTheme();
                        break;
                    case "OrangePurple.xaml":
                        ApplyOrangePurpleTheme();
                        break;
                    case "GreenPurple.xaml":
                        ApplyGreenPurpleTheme();
                        break;
                    case "BlueRed.xaml":
                        ApplyBlueRedTheme();
                        break;
                    case "BluePink.xaml":
                        ApplyBluePinkTheme();
                        break;
                    case "YellowPink.xaml":
                        ApplyYellowPinkTheme();
                        break;
                    case "BrownBlue.xaml":
                        ApplyBrownBlueTheme();
                        break;
                    case "DefaultTheme.xaml":
                        ApplyDefaultTheme();
                        break;
                    // Add more cases for other themes if needed
                    default:
                        // Apply default theme
                        ApplyDefaultTheme();
                        break;
                }
            }
        }

        void ApplyYellowBlueTheme()
        {
            // Update resource values for the YellowBlue theme
            App.Current.Resources["priColor"] = (Color)(App.Current.Resources["yellowColor"]);
            App.Current.Resources["secColor"] = (Color)(App.Current.Resources["blueColor"]);
            App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightBlueColor"]);
            App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightYellowColor"]);

            App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["blueColor"]);
            App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["yellowColor"]);
            // Add more color updates as needed

            // Optionally, update other resource values like styles, etc.

            // Save the theme preference
            SaveThemePreference("YellowBlue.xaml");
        }

        void ApplyTanTurquoiseTheme()
        {
            App.Current.Resources["priColor"] = (Color)(App.Current.Resources["tanColor"]);
            App.Current.Resources["secColor"] = (Color)(App.Current.Resources["turquoiseColor"]);
            App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightTurquoiseColor"]);
            App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightTanColor"]);

            App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["turquoiseColor"]);
            App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["tanColor"]);

            SaveThemePreference("TanTurquoise.xaml");

        }

        void ApplyOrangePurpleTheme()
        {
            App.Current.Resources["priColor"] = (Color)(App.Current.Resources["orangeColor"]);
            App.Current.Resources["secColor"] = (Color)(App.Current.Resources["purpleColor"]);
            App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightPurpleColor"]);
            App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightOrangeColor"]);

            App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["purpleColor"]);
            App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["orangeColor"]);
            SaveThemePreference("OrangePurple.xaml");
        }

        void ApplyGreenPurpleTheme()
        {
            App.Current.Resources["priColor"] = (Color)(App.Current.Resources["greenColor"]);
            App.Current.Resources["secColor"] = (Color)(App.Current.Resources["purpleColor"]);
            App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightPurpleColor"]);
            App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightGreenColor"]);

            App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["purpleColor"]);
            App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["greenColor"]);
            SaveThemePreference("GreenPurple.xaml");
        }

        void ApplyBlueRedTheme()
        {
            App.Current.Resources["priColor"] = (Color)(App.Current.Resources["blueColor"]);
            App.Current.Resources["secColor"] = (Color)(App.Current.Resources["redColor"]);
            App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightRedColor"]);
            App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightBlueColor"]);

            App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["redColor"]);
            App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["blueColor"]);
            SaveThemePreference("BlueRed.xaml");
        }

        void ApplyBluePinkTheme()
        {
            App.Current.Resources["priColor"] = (Color)(App.Current.Resources["blueColor"]);
            App.Current.Resources["secColor"] = (Color)(App.Current.Resources["pinkColor"]);
            App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightPinkColor"]);
            App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightBlueColor"]);

            App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["pinkColor"]);
            App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["blueColor"]);
            SaveThemePreference("BluePink.xaml");
        }

        void ApplyYellowPinkTheme()
        {
            App.Current.Resources["priColor"] = (Color)(App.Current.Resources["yellowColor"]);
            App.Current.Resources["secColor"] = (Color)(App.Current.Resources["pinkColor"]);
            App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightPinkColor"]);
            App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightYellowColor"]);

            App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["pinkColor"]);
            App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["yellowColor"]);
            SaveThemePreference("YellowPink.xaml");
        }

        void ApplyBrownBlueTheme()
        {
            App.Current.Resources["priColor"] = (Color)(App.Current.Resources["brownColor"]);
            App.Current.Resources["secColor"] = (Color)(App.Current.Resources["blueColor"]);
            App.Current.Resources["triColor"] = (Color)(App.Current.Resources["lightBlueColor"]);
            App.Current.Resources["quadColor"] = (Color)(App.Current.Resources["lightBrownColor"]);

            App.Current.Resources["tempTri"] = (Color)(App.Current.Resources["blueColor"]);
            App.Current.Resources["tempQuad"] = (Color)(App.Current.Resources["brownColor"]);
            Preferences.Set("ThemePreference", "BrownBlue.xaml");
        }

        void ApplyDefaultTheme()
        {
            App.Current.Resources["priColor"] = (Color)(Color.FromArgb("#74E291"));
            App.Current.Resources["secColor"] = (Color)(Color.FromArgb("#CCF2D1"));
            App.Current.Resources["triColor"] = (Color)(Color.FromArgb("#f1eee8"));
            App.Current.Resources["quadColor"] = (Color)(Color.FromArgb("#f1eee8"));

            App.Current.Resources["tempTri"] = (Color)(Color.FromArgb("#f1eee8"));
            App.Current.Resources["tempQuad"] = (Color)(Color.FromArgb("#000000"));
            SaveThemePreference("DefaultTheme.xaml");
        }

        void SaveThemePreference(string themeName)
        {
            Preferences.Set(ThemePreferenceKey, themeName);
        }
    }
}
