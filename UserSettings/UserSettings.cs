using SQLite;

namespace UserSettings;
public partial class UserSettingsPage
{
    public class UserSet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } //MAYBE JUST TRACK 1 USER
        public string Name { get; set; }
        public int Age { get; set; }
        public bool lightOrDark { get; set; }

        public string SomeEntry { get; set; }

        //public string Email { get; set; } <- RISK
        
        //public DatePicker DateSelected { get; set; }
    }
}