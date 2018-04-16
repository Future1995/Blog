using System;

namespace HwaTec.Blog.Model
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Content { get; set; }
        public int CreateId { get; set; }
        public int TypeId { get; set; }
    }
}
