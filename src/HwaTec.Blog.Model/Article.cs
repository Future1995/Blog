using System;

namespace HwaTec.Blog.Model
{
    public class Article: MongoEntityBase
    {
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Synopsis { get; set; }
        public string Content { get; set; }
        public Guid CreateId { get; set; }
        public int TypeId { get; set; }
    }
}
