using creditPreCheck.Controllers;
using creditPreCheck.Data;
using creditPreCheck.Models;
using creditPreCheck.Utils;
using creditPreCheck.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using MemoryCache.Testing.Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace creditPreCheck.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private Mock<ILogger<HomeController>> _logger;
        private Mock<IDataManager> _dataManager;
        private IMemoryCache _memoryCache;
        private Mock<IUtil> _utils;
        private string cacheKey;
        private HomeController _controller;

        [TestInitialize]
        public void SetUp()
        {
            cacheKey = Stubs.GetCacheKey();
            var mockedCache = Create.MockedMemoryCache();
            _dataManager = new Mock<IDataManager>();
            _logger = new Mock<ILogger<HomeController>>();
            _memoryCache = mockedCache;
            _utils = new Mock<IUtil>();

            _controller = new HomeController(
              _dataManager.Object,
              _logger.Object,
              _utils.Object,
              _memoryCache
            )
            {
                TempData = new Mock<TempDataDictionary>(new DefaultHttpContext(), Mock.Of<ITempDataProvider>()).Object
            };
        }

        [TestMethod]
        public void Renders_Landing_Page_Correctly()
        {
            var result = _controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void SubmitsUserInfoWithCorrectData()
        {
            User user = Stubs.GetInputUser();
            User newUser = Stubs.GetFullUser();
            _dataManager.Setup(x => x.AddUser(It.IsAny<User>(), It.IsAny<string>())).Returns(newUser);
            _utils.Setup(x => x.CreateCacheKey(It.IsAny<User>())).Returns(cacheKey);

            var result = _controller.Index(user) as RedirectToActionResult;
            Assert.AreEqual("Result", result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void ShowsErrorsWithIncorrectData()
        {
            User user = Stubs.GetIncorrectInputUser();
            _controller.ModelState.AddModelError("LastName", "LastNameErrorMessage");

            var result = _controller.Index(user) as ViewResult;
            var errModel = result.ViewData.ModelState;

            Assert.AreEqual(false, errModel.IsValid);
            Assert.AreEqual(1, errModel.ErrorCount);
            Assert.AreEqual("LastName", errModel.Keys.ToList().First());
            Assert.AreEqual("LastNameErrorMessage", errModel.Values.First().Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void SubmitsUserInfoWithDataFromCache()
        {
            User user = Stubs.GetInputUser();
            User newUser = Stubs.GetFullUser();
            _dataManager.Setup(x => x.AddUser(It.IsAny<User>(), It.IsAny<string>())).Returns(newUser);
            _utils.Setup(x => x.CreateCacheKey(It.IsAny<User>())).Returns(cacheKey);
            _memoryCache.Set(cacheKey, newUser);
            
            var result = _controller.Index(user) as RedirectToActionResult;
            Assert.AreEqual("Result", result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void SubmitsUserInfoForCacheExpiredUser()
        {
            User user = Stubs.GetInputUser();
            User newUser = Stubs.GetFullUser();
            newUser.CreatedOn = DateTime.Today.AddDays(-1);

            _dataManager.Setup(x => x.AddUser(It.IsAny<User>(), It.IsAny<string>())).Returns(newUser);
            _utils.Setup(x => x.CreateCacheKey(It.IsAny<User>())).Returns(cacheKey);
            _utils.Setup(x => x.IsUserCacheExpired(It.IsAny<User>())).Returns(true);
            _memoryCache.Set(cacheKey, newUser);

            var result = _controller.Index(user) as RedirectToActionResult;
            Assert.AreEqual("Result", result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
