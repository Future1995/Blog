using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HwaTec.Blog.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace HwaTec.Blog.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        private IMemoryCache _memoryCache;
        public BaseController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public UserInfo LoginUser { get; private set; }
        // GET: Base
        //执行控制器方法之前先执行该方法
        //获取sessionid的key，然后从_memoryCache里取出来验证
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isExt = false;
            if (Request.Cookies["sessionId"] != null)
            {
                string sessionId = Request.Cookies["sessionId"] as string;//取出cookies里的key
                var userInfo = _memoryCache.Get<UserInfo>(sessionId);
                if (userInfo != null)
                {
                    LoginUser = userInfo;
                    isExt = true;
                }
            }
            if (!isExt)//用户没有登录
            {
                filterContext.HttpContext.Response.Redirect("/Admin/Account/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}