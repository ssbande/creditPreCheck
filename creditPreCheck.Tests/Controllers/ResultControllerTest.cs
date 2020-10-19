using creditPreCheck.Controllers;
using creditPreCheck.Data;
using creditPreCheck.Models;
using creditPreCheck.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MemoryCache.Testing.Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace creditPreCheck.Tests.Controllers
{
    [TestClass]
    public class ResultControllerTest
    {
        private Mock<ILogger<ResultController>> _logger;
        private Mock<IDataManager> _dataManager;
        private IMemoryCache _memoryCache;
        private ResultController _controller;

        [TestInitialize]
        public void SetUp()
        {
            var mockedCache = Create.MockedMemoryCache();
            _dataManager = new Mock<IDataManager>();
            _logger = new Mock<ILogger<ResultController>>();
            _memoryCache = mockedCache;

            _controller = new ResultController(
              _logger.Object,
              _dataManager.Object,
              _memoryCache
            )
            {
                TempData = new Mock<TempDataDictionary>(new DefaultHttpContext(), Mock.Of<ITempDataProvider>()).Object
            };
        }

        [TestMethod]
        public void RendersDataForTheSubmittedUserFromDB()
        {
            User newUser = Stubs.GetFullUser();
            _controller.ControllerContext = Stubs.GetControllerContext();
            _dataManager.Setup(x => x.GetLastEntry()).Returns(newUser);

            ViewResult result = _controller.Index() as ViewResult;
            User model = result.ViewData.Model as User;

            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual("TestFirstName", model.FirstName);
            Assert.AreEqual("TestLastName", model.LastName);
            Assert.AreEqual(true, model.Eligibility.isEligible);
            Assert.AreEqual("Barclays Card", model.Eligibility.Card.Name);
        }

        [TestMethod]
        public void RedirectsToHomeWhenNoReferrer()
        {
            _controller.ControllerContext = Stubs.GetControllerContext("");

            var result = _controller.Index() as RedirectToActionResult;
            Assert.AreEqual("Home", result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void ReadsDataFromCacheWhenUserInCache()
        {
            User newUser = Stubs.GetFullUser();
            _controller.ControllerContext = Stubs.GetControllerContext();
            _controller.TempData["cacheUser"] = Stubs.GetCacheKey();

            _memoryCache.Set(Stubs.GetCacheKey(), newUser);
            ViewResult result = _controller.Index() as ViewResult;
            User model = result.ViewData.Model as User;

            Assert.AreEqual("Index", result.ViewName);
            Assert.AreEqual("TestFirstName", model.FirstName);
            Assert.AreEqual("TestLastName", model.LastName);
            Assert.AreEqual(true, model.Eligibility.isEligible);
            Assert.AreEqual("Barclays Card", model.Eligibility.Card.Name);
        }
    }
}