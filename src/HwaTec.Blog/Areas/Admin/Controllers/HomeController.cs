using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HwaTec.Blog.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HwaTec.Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IMemoryCache _memoryCache;
        public HomeController(IArticleService articleService, IMemoryCache memoryCache) : base(memoryCache)
        {
            _articleService = articleService;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            var totalCount = 0;
            var articles = _articleService.LoadEntities(out totalCount);
            return View(articles);
        }

    }
}