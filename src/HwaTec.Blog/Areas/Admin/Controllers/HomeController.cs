using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HwaTec.Blog.Service;
using Microsoft.AspNetCore.Mvc;

namespace HwaTec.Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public IActionResult Index()
        {
            var totalCount = 0;
            var articles = _articleService.LoadEntities(out totalCount);
            return View(articles);
        }
    
    }
}