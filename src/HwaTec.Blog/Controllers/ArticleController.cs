using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HwaTec.Blog.Model;
using HwaTec.Blog.Service;
using Microsoft.AspNetCore.Mvc;

namespace HwaTec.Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public IActionResult Index()
        {
            var totalCount = 0;
            var articles = _articleService.LoadEntities(out totalCount);
            return View(articles);
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
            return Redirect("/Article/Index");
        }
        public IActionResult Details(int id)
        {
            var article = _articleService.GetById(id);
            return View(article);
        }
    }
}