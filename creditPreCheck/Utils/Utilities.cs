using System;
using System.Linq;
using creditPreCheck.Models;

namespace creditPreCheck.Utils
{
    public interface IUtil
    {
        string CreateCacheKey(User user);
        int GetAge(User user);
        string GetCamelFirstName(string firstName);
        string GetCamelLastName(string lastName);
        bool IsEligibleForCard(User user);
        bool IsUserCacheExpired(User user);
        string LogSavingUser(User user);
        string LogUserInCache(User user);
    }

    public class Utilities : IUtil
    {
        public string CreateCacheKey(User user)
        {
            var fName = user.FirstName.Trim().ToLower();
            var lName = user.LastName.Trim().ToLower();
            var dob = user.DoB.Date.ToString("yyyyMMdd");
            return $"{fName}{lName}{dob}";
        }

        public int GetAge(User user)
        {
            int age = DateTime.Now.Year - user.DoB.Year;
            return (DateTime.Now.DayOfYear < user.DoB.DayOfYear)
                ? age - 1
                : age;
        }

        public string GetCamelFirstName(string firstName)
        {
            var firstCap = firstName.First().ToString().ToUpper();
            return $"{firstCap}{firstName.Substring(1)}";
        }

        public string GetCamelLastName(string lastName)
        {
            var firstCap = lastName.First().ToString().ToUpper();
            return $"{firstCap}{lastName.Substring(1)}";
        }

        public bool IsEligibleForCard(User user)
        {
            return this.GetAge(user) >= Config.minAge;
        }

        public bool IsUserCacheExpired(User user)
        {
            var hoursSinceLastChecked = (DateTime.Now - user.CreatedOn).TotalHours;
            return hoursSinceLastChecked > Config.cacheExpiryHours;
        }

        public string LogSavingUser(User user)
        {
            var allottedCard = user.Eligibility.Card != null
                ? user.Eligibility.Card.Name
                : "None";

            return string.Format(Config.addUserLogMsg,
                user.FullName,
                user.Eligibility.isEligible,
                allottedCard);
        }

        public string LogUserInCache(User user)
        {
            var allottedCard = user.Eligibility.Card != null
                ? user.Eligibility.Card.Name
                : "None";

            return string.Format(Config.cacheUserLogMsg,
                user.FullName,
                user.Eligibility.isEligible,
                allottedCard);
        }
    }
}
