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
    public class ArticleController : BaseController
    {


        private readonly IArticleService _articleService;
        private readonly IMemoryCache _memoryCache;
        public ArticleController(IArticleService articleService, IMemoryCache memoryCache):base(memoryCache)
        {
            _articleService = articleService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title, string synopsis, string content)
        {
            Article article = new Article
            {
                CreateTime = DateTime.Now,
                Title = title,
                Synopsis = synopsis,
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

        [HttpGet]
        public IActionResult GetArticles()
        {
            var totalCount = 0;
            var articles = _articleService.LoadEntities(out totalCount);
            return Json(new { code = 0, msg = "", count = totalCount, data = articles });
        }

        public IActionResult Update(int id)
        {
            var article = _articleService.GetById(id);
            return View(article);
        }
        [HttpPost]
        public IActionResult Update(Article article)
        {
            article.ModifyTime = DateTime.Now;
            _articleService.Update(article);
            return View(article);
        }

        [HttpPost]
        public IActionResult Delete(int[] ids)
        {
            _articleService.Delete(ids);
            return Json("ok");
        }

 

    }
}