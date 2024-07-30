namespace climb_higher;

public partial class PrivacyPage : ContentPage
{
    public PrivacyPage()
    {
        InitializeComponent();

        dataStorageLabel.Text = "The application stores user-entered data (such as\n" +
                                "climb information, stopwatch entries, color theme\n" +
                                "preference, etc) locally to the Android device.\n" +
                                "No external machines are used to store data the\n" +
                                "user has entered. This gives the user complete\n" +
                                "control over their data.";

        dataRetentionLabel.Text =   "As mentioned in the above section, user-entered\n" +
                                    "data is stored locally on the users device.\n" +
                                    "Therefore, data is retained as long as the user has\n" +
                                    "the application installed. If the user wishes to\n" +
                                    "remove their data from the application's storage\n" +
                                    "they can simply uninstall the application. Individual\n" +
                                    "components of user-entered data may be deleted using the user interface.";

        dataProcessingLabel.Text =  "The user's data (such as climb times and images),\n" +
                                    "are processed to provide the user information\n" +
                                    "such as  statistics and identification of holds.\n" +
                                    "The image processing makes use of the 'Emgu CV'\n" +
                                    "library. This library process the image locally\n" +
                                    "on the user's device and does not call any APIs\n" +
                                    "or third party services to perform the processing\n" +
                                    "Once processed the resulting image is not stored.";
    }
}