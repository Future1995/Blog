﻿using System;

namespace HwaTec.Blog.MongoRep
{
    public abstract class MongodbSettings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LoginDatabase { get; set; }
    }
}
