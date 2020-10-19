using System;
using creditPreCheck.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace creditPreCheck.Tests.Utils
{
    [TestClass]
    public class ValidationsTest
    {
        [TestMethod]
        public void CanCheckForFurtureDateValidation()
        {
            DateTime dateToCheck = DateTime.Today.AddDays(-1);
            var attr = new FutureDateAttribute();

            var result = attr.IsValid(dateToCheck);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CanCheckForFurtureDateValidationOnFutureDate()
        {
            DateTime dateToCheck = DateTime.Today.AddDays(1);
            var attr = new FutureDateAttribute();

            var result = attr.IsValid(dateToCheck);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void CanCheckForOldDateValidation()
        {
            DateTime dateToCheck = DateTime.Parse("01-01-2000");
            var attr = new OldestDateAttribute();

            var result = attr.IsValid(dateToCheck);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CanCheckForOldDateValidationOnPastDate()
        {
            DateTime dateToCheck = DateTime.Parse("01-01-1800");
            var attr = new OldestDateAttribute();

            var result = attr.IsValid(dateToCheck);
            Assert.AreEqual(false, result);
        }
    }
}
