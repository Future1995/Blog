using HwaTec.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HwaTec.Blog.Service
{
    public interface IArticleService
    {

        IQueryable<Article> LoadEntities(out int totalCount,int pageIndex=1, int pageSize=10);
        Article GetById(int id);

        Article Add(Article article);

        bool Update(Article article);

        bool Delete(IEnumerable<int> ids);
    }
}
