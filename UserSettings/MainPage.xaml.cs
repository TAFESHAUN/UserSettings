using SQLite;
using static UserSettings.UserSettingsPage;

namespace UserSettings;

public partial class MainPage : ContentPage
{
	int count = 0;
	UserSet setLoad = new UserSet();

    private SQLiteAsyncConnection _database;
    public MainPage()
	{
		InitializeComponent();
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "settings.db");
        _database = new SQLiteAsyncConnection(databasePath);
        var existingSettings =  _database.Table<UserSet>().FirstOrDefaultAsync();
		setLoad = existingSettings.Result;

		nameSettingsSave.Text = setLoad.Name;
		emailSavedSettings.Text = setLoad.SomeEntry; //Changed to email
		//Showcasing some entry being saved

		//Could this this global SQLite settings to test admin priv
			//-> CRUD locked until settings set etc.
		//Onload of app please set your settings for the first etc.
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

