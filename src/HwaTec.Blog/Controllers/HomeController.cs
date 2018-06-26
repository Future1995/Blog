using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HwaTec.Blog.Model;
using HwaTec.Blog.MongoRep;
using Microsoft.AspNetCore.Mvc;

namespace HwaTec.Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly NoSqlBaseRepository<Article> _articleRep;
        public HomeController(NoSqlBaseRepository<Article> articleRep)
        {
            _articleRep = articleRep;
        }
        public IActionResult Index()
        {
            var totalCount = 0;
            var articles = _articleRep.GetAll();
            return View(articles);
        }

        public IActionResult Details(int id)
        {
            var article = _articleRep.GetById(id);
            return View(article);
        }
    }
}