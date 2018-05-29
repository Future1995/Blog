using System;
using System.Collections.Generic;
using System.Text;

namespace HwaTec.Blog.Model
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string UserCode { get; set; }
        public string UserNick { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string  HeadImage { get; set; }
    }
}
