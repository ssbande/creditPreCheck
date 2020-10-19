using System;
using creditPreCheck.Models;
using creditPreCheck.Utils;
using creditPreCheck.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace creditPreCheck.Tests.Utils
{
    [TestClass]
    public class UtilitiesTest
    {
        private IUtil _utils;
        private User _user;

        [TestInitialize]
        public void SetUp()
        {
            _utils = new Utilities();
            _user = Stubs.GetFullUser();
        }

        [TestMethod]
        public void CanCreateCacheKey()
        {
            var result = _utils.CreateCacheKey(_user);
            Assert.AreEqual(Stubs.GetCacheKey().ToLower(), result);
        }

        [TestMethod]
        public void CanCalculateAge()
        {
            int age = _utils.GetAge(_user);
            Assert.AreEqual(20, age);
        }

        [TestMethod]
        public void CanGetCamelFirstName()
        {
            string result = _utils.GetCamelFirstName("firstName");
            Assert.AreEqual("FirstName", result);
        }

        [TestMethod]
        public void CanGetCamelLastName()
        {
            string result = _utils.GetCamelLastName("lastName");
            Assert.AreEqual("LastName", result);
        }

        [TestMethod]
        public void CanCheckUserEligibilityForCard()
        {
            bool result = _utils.IsEligibleForCard(_user);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CanCheckUserIneligibilityForCard()
        {
            _user.DoB = DateTime.Parse("01-01-2010");
            bool result = _utils.IsEligibleForCard(_user);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CanCheckUserUserCacheExpiry()
        {
            _user.CreatedOn = DateTime.Today.AddDays(-1);
            bool result = _utils.IsUserCacheExpired(_user);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CanCheckUserUserCacheInexpiry()
        {
            _user.CreatedOn = DateTime.Now;
            bool result = _utils.IsUserCacheExpired(_user);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CanCreateMessageForSavingLogsForValidCard()
        {
            string result = _utils.LogSavingUser(_user);
            Assert.AreEqual("Adding user to DB, TestFirstName TestLastName eligible: True, card: Barclays Card.", result);
        }

        [TestMethod]
        public void CanCreateMessageForSavingLogsForNoCard()
        {
            _user.Eligibility.isEligible = false;
            _user.Eligibility.Card = null;
            string result = _utils.LogSavingUser(_user);
            Assert.AreEqual("Adding user to DB, TestFirstName TestLastName eligible: False, card: None.", result);
        }

        [TestMethod]
        public void CanCreateMessageForCacheLogsForValidCard()
        {
            string result = _utils.LogUserInCache(_user);
            Assert.AreEqual("User present in cache, TestFirstName TestLastName eligible: True, card: Barclays Card.", result);
        }

        [TestMethod]
        public void CanCreateMessageForCacheLogsForNoCard()
        {
            _user.Eligibility.isEligible = false;
            _user.Eligibility.Card = null;
            string result = _utils.LogUserInCache(_user);
            Assert.AreEqual("User present in cache, TestFirstName TestLastName eligible: False, card: None.", result);
        }
    }
}
