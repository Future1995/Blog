using System;
using System.Linq;
using HwaTec.Blog.Model;
using HwaTec.Blog.Repository;

namespace HwaTec.Blog.Service
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> _articleRepository;
        public ArticleService(IRepository<Article> articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public Article Add(Article article)
        {
            return _articleRepository.AddEntity(article);
        }

        public Article GetById(int id)
        {
            return _articleRepository.GetById(id);
        }

        public IQueryable<Article> LoadEntities(out int totalCount, int pageIndex, int pageSize)
        {
            return _articleRepository.LoadPageEntities(pageIndex, pageSize, out totalCount, null, a => a.Title, true);
        }

        public bool Update(Article article)
        {
           return _articleRepository.UpdateEntity(article);
        }
    }
}
