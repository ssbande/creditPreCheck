namespace creditPreCheck.Utils
{
    public static class Config
    {
        public static string oldestDate = "Jan 1, 1900";
        public static int minAge = 18;
        public static int incomeBorder = 30000;
        public static int cacheExpiryHours = 6;
        public static string cacheUserLogMsg = "User present in cache, {0} eligible: {1}, card: {2}.";
        public static string addUserLogMsg = "Adding user to DB, {0} eligible: {1}, card: {2}.";
        public static string dbFetchErrorMsg = "Error while fetching user from database";
    }
}
