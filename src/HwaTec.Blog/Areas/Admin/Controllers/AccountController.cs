using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HwaTec.Blog.Model;
using HwaTec.Blog.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HwaTec.Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private IUserInfoService _userInfoService;
        private IMemoryCache _memoryCache;
        public AccountController(IUserInfoService userInfoService, IMemoryCache memoryCache)
        {
            _userInfoService = userInfoService;
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string userName, string password)
        {

            var userInfo = _userInfoService.Login(userName, password);
            if (userInfo != null)
            {
                var sessionId = Guid.NewGuid().ToString();
                this.HttpContext.Response.Cookies.Append("sessionId", sessionId);
                _memoryCache.Set(sessionId, userInfo, new TimeSpan(24, 0, 0));
                return Redirect("/Admin/Home/Index");
            }
            else
            {
                return Json("no");
            }
        }
    }
}