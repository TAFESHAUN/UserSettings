using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SQLite;
using static UserSettings.UserSettingsPage;

namespace UserSettings;

public partial class App : Application
{
    /// <summary>
    /// To improve this you could update Settings database to repo class format
    /// </summary>
    private SQLiteAsyncConnection _database;
    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();


        // Initialize the SQLite database connection
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "settings.db");
        _database = new SQLiteAsyncConnection(databasePath);
        _database.CreateTableAsync<UserSet>().Wait();

        // Load the user settings
        LoadUserSettings();
    }

    private async void LoadUserSettings()
    {
        // Check if the user settings already exist in the database
        var existingSettings = await _database.Table<UserSet>().FirstOrDefaultAsync();
        if (existingSettings != null)
        {
            var currentTheme = existingSettings.lightOrDark;
            if (currentTheme)
                Application.Current.UserAppTheme = AppTheme.Dark;
            else
                Application.Current.UserAppTheme = AppTheme.Light;
        }
    }
}
