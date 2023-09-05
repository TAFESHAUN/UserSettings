using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using SQLite;
using System;
using System.IO;
using System.Linq;

namespace UserSettings;
/// <summary>
/// We could also save these settings to CSV or some Webservice but SQLite is fast, easy and feature rich
/// If we want to use defaults we can simply use MAUI preferences
/// OR a combo of both!
/// LINK: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences?tabs=windows
/// </summary>
public partial class UserSettingsPage : ContentPage
{
    //Data that you don't want exposed.
    private SQLiteAsyncConnection _database;

    //REPO -> PASS THE REPO, We do the connection in that repo.
    public UserSettingsPage() //Page Construct I COULD have the global settings DB
	{
		InitializeComponent();

        // Initialize the SQLite database connection
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "settings.db");
        _database = new SQLiteAsyncConnection(databasePath);
        _database.CreateTableAsync<UserSet>().Wait();
        //_database.DeleteAllAsync<UserSet>().Wait();

        // Load the user settings
        LoadUserSettings();
    }
    private async void LoadUserSettings() //TASK -> Command MVVM -> MVC
    {
        // Check if the user settings already exist in the database
        var existingSettings = await _database.Table<UserSet>().FirstOrDefaultAsync();
        if (existingSettings != null)
        {
            NameEntry.Text = existingSettings.Name;
            AgeEntry.Text = existingSettings.Age.ToString();

            someEntry.Text = existingSettings.SomeEntry.ToString();
            //LOAD EMAIL <- RISK
            //LAST DATE LOGGED
            //USER ID -> UNQ Code -> Student Numb, Employee Num
            //Logging for hours -> Timesheet alignment, Clock in and off.
            //Roster -> Days of the week, Loaded from a db -> Save the days working this week to sqlite.

            if (existingSettings.lightOrDark)
            {
                togTheme.IsToggled = true;
            }
            else
            {
                togTheme.IsToggled = false;
            }

            var currentTheme = existingSettings.lightOrDark;
            if (currentTheme)
                Application.Current.UserAppTheme = AppTheme.Dark; 
                //YOU TELL THE USER ADMIN OR SYSTEM PREFS -> when dl needs these.
                //Prompt, Setup device settings now after register.
                //Setup now (Highlight btn, skip not highlighted).
            else
                Application.Current.UserAppTheme = AppTheme.Light;
        }
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text;
        int age = 0;
        int.TryParse(AgeEntry.Text, out age);
        bool theme = togTheme.IsToggled;
        var someEnt = someEntry.Text;

        var userSettings = new UserSet
        {
            Name = name,
            Age = age,
            lightOrDark = theme,
            SomeEntry = someEnt
        };

        await _database.InsertOrReplaceAsync(userSettings);

        // Show a confirmation message
        await DisplayAlert("Success", "User settings saved", "OK");
    }

    //PREFERENCES
    //SWITCH: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/switch
    private void OnThemeSwitchToggled(object sender, ToggledEventArgs e)
    {
        bool isDarkTheme = e.Value;
        Preferences.Set("DarkThemeOn", isDarkTheme ? "Dark" : "Light");

        // Apply the theme
        if (isDarkTheme)
            Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Application.Current.UserAppTheme = AppTheme.Light;
    }
}