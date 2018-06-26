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
    public class HomeController : BaseController
    {
        private readonly NoSqlBaseRepository<Article> _articleRep;
        private readonly IMemoryCache _memoryCache;
        public HomeController(NoSqlBaseRepository<Article> articleRep, IMemoryCache memoryCache) : base(memoryCache)
        {
            _articleRep = articleRep;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            if (LoginUser == null)
                return Redirect("/Admin/Account/Login");
            var totalCount = 0;
            //var query = _articleRep.LoadEntities(out totalCount);
            //var articles = from a in query
            //               where a.CreateId == LoginUser.Id
            //               select a;
            var articles = _articleRep.GetAll();
            return View(articles);
        }

    }
}