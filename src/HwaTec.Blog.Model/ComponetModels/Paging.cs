using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwaTec.Blog.Model
{
    /// <summary>
    /// 分页结果数据对象
    /// </summary>
    public class Paging : IDto
    {
        public Paging()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get { return PageSize > 0 ? (Total / PageSize + (Total % PageSize > 0 ? 1 : 0)) : 0; } }
        public int Total { get; set; }
    }
}
