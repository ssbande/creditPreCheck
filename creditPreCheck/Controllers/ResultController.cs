using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using creditPreCheck.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using creditPreCheck.Data;
using creditPreCheck.Utils;

namespace creditPreCheck.Controllers
{
    public class ResultController : Controller
    {
        private readonly ILogger<ResultController> _logger;
        private readonly IDataManager _dataManager;
        private readonly IMemoryCache _memoryCache;

        public ResultController(ILogger<ResultController> logger, IDataManager dataManager, IMemoryCache memoryCache)
        {
            _logger = logger;
            _dataManager = dataManager;
            _memoryCache = memoryCache;
        }

        [HandleException]
        public IActionResult Index()
        {
            try
            {
                var referrer = Request.Headers["Referer"].ToString();
                if (referrer == "") return RedirectToAction("Index", "Home");
                var cacheKey = TempData["cacheUser"];
                var lastEntry = cacheKey != null
                    ? _memoryCache.Get<User>(cacheKey)
                    : _dataManager.GetLastEntry();
                return View("Index", lastEntry);
            }
            catch (System.Exception)
            {
                _logger.LogError(Config.dbFetchErrorMsg);
                throw new Exception(Config.dbFetchErrorMsg);
            }
        }
    }
}