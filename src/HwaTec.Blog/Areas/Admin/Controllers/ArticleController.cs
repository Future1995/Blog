using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HwaTec.Blog.Model;
using HwaTec.Blog.Service;
using Microsoft.AspNetCore.Mvc;

namespace HwaTec.Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title, string content)
        {
            Article article = new Article
            {
                CreateTime = DateTime.Now,

                Title = title,
                Content = content,
                ModifyTime = DateTime.Now
            };
            _articleService.Add(article);
            return Json("ok");
        }
        public IActionResult Details(int id)
        {
            var article = _articleService.GetById(id);
            return View(article);
        }
    }
}