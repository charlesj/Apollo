using Remotion.Linq.Clauses.ResultOperators;

namespace Apollo
{
    public static class Constants
    {
        public static class EnvironmentalVars
        {
            public const string DatabaseName = "APOLLO_DATABASENAME";
            public const string DatabaseServer = "APOLLO_DATABASESERVER";
            public const string DatabaseUsername = "APOLLO_DATABASEUSERNAME";
            public const string DatabasePassword = "APOLLO_DATABASEPASSWORD";
            public const string LoginHash = "APOLLO_LOGINHASH";
        }

        public static class UserSettings
        {
            public const string DarkSkyApiKey = "DarkSkyApiKey";
            public const string PasswordHash = "password_hash";
        }

        public static class Documents
        {
            public const string WeatherLocations = "weatherLocations";
        }

        public static class TimelineReferences
        {
            public const string Bookmark = "bookmark";
            public const string Checklist = "checklist";
            public const string ChecklistCompletion = "checklistCompletion";
            public const string Feed = "feed";
            public const string Journal = "journal";
            public const string Note = "note";
        }
    }
}
