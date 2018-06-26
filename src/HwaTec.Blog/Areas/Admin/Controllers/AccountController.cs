using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HwaTec.Blog.Model;
using HwaTec.Blog.MongoRep;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HwaTec.Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private NoSqlBaseRepository<UserInfo> _userInfoRep;
        private IMemoryCache _memoryCache;
        public AccountController(NoSqlBaseRepository<UserInfo> userInfoRep, IMemoryCache memoryCache)
        {
            _userInfoRep = userInfoRep;
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

            var userInfo = _userInfoRep.GetByProperty(x => x.UserCode, userName);
            if (userInfo != null && userInfo.Password == password)
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

        public IActionResult InitUser()
        {
            try
            {
                _userInfoRep.Add(new UserInfo { UserCode = "shitong", Password = "shitong" });
            }
            catch (Exception)
            {
                return Content("Error");
            }
            return Content("ok");
        }
    }
}