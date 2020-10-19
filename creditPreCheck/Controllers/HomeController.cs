using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using creditPreCheck.Models;
using creditPreCheck.Utils;
using creditPreCheck.Data;

namespace creditPreCheck.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataManager _dataManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IUtil _utils;

        public HomeController(IDataManager dataManager, ILogger<HomeController> logger, IUtil utils, IMemoryCache memoryCache)
        {
            _logger = logger;
            _dataManager = dataManager;
            _utils = utils;
            _memoryCache = memoryCache;
        }

        public ViewResult Index()
        {
            ModelState.Clear();
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleException]
        public IActionResult Index([Bind("UserId,FirstName,LastName,DoB,Income")] User user)
        {
            if (!ModelState.IsValid) return View("Index", user);

            var cacheKey = _utils.CreateCacheKey(user);
            var isUserInCache = _memoryCache.TryGetValue(cacheKey, out User cacheUser);

            if (isUserInCache)
            {
                if (_utils.IsUserCacheExpired(cacheUser)) this.AddUserToDb(user, cacheKey);
                else
                {
                    _logger.LogInformation(_utils.LogUserInCache(user));
                    TempData["cacheUser"] = cacheKey;
                }
            }
            else this.AddUserToDb(user, cacheKey);
            return RedirectToAction("Index", "Result");
        }

        public void AddUserToDb(User user, string cacheKey)
        {
            try
            {
                var newUser = _dataManager.AddUser(user, cacheKey);
                _memoryCache.Set(cacheKey, newUser);
                _logger.LogInformation(_utils.LogSavingUser(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
