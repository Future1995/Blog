using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwaTec.Blog.Model
{
    /// <summary>
    /// 分页数据返回对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T> : IDto
    {
        /// <summary>
        /// 分页的结果信息
        /// </summary>
        public Paging Paging { get; set; }

        /// <summary>
        /// 分页之后的数据集合
        /// </summary>

        [JsonProperty("data")]
        public List<T> Records { get; set; }
    }

    public static class PageExtensions
    {
        public static object ToJqueryDatatableFormat<T>(this Page<T> obj, int draw = 0)
        {
            return new { data = obj.Records, recordsTotal = obj.Paging.Total, recordsFiltered = obj.Records.Count, draw = draw };
        }

        public static object JqueryDatatableError(string error)
        {
            return new { error = error };
        }
    }
}
