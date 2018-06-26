using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace HwaTec.Blog.Model
{
    public class MongoEntityBase : IEntityBase
    {
        public MongoEntityBase()
        {
            Sysid = Guid.NewGuid();
        }

        #region Properties
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Sysid { get; set; }
        #endregion
    }
}
