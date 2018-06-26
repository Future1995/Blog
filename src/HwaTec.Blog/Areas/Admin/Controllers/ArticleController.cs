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
    public class ArticleController : BaseController
    {

        private readonly NoSqlBaseRepository<Article> _articleRep;
        private readonly IMemoryCache _memoryCache;
        public ArticleController(NoSqlBaseRepository<Article> articleRep, IMemoryCache memoryCache) : base(memoryCache)
        {
            _articleRep = articleRep;
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
                ModifyTime = DateTime.Now,
                CreateId = this.LoginUser.Sysid
            };
            _articleRep.Add(article);
            return Json("ok");
        }
        public IActionResult Details(int id)
        {
            var article = _articleRep.GetById(id);
            return View(article);
        }
        [HttpGet]
        public IActionResult GetArticles()
        {
            //var totalCount = 0;
            //var query = _articleRep.LoadEntities(out totalCount);
            //var articles = from a in query
            //               where a.CreateId == LoginUser.Id
            //               select a;
           var articles= _articleRep.GetAll();
           
            return Json(new { code = 0, msg = "", count = articles.Count() , data = articles });
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var article = _articleRep.GetById(id);
            return View(article);
        }
        [HttpPost]
        public IActionResult Update(Article article)
        {
            article.ModifyTime = DateTime.Now;
            _articleRep.Update(article);
            return Json("ok");
        }
        [HttpPost]
        public IActionResult Delete(object[] ids)
        {
            _articleRep.DeleteEntities(ids);
            return Json("ok");
        }

    }
}