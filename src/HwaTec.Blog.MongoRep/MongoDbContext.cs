using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;

namespace HwaTec.Blog.MongoRep
{
    public class MongoDbContext<TConnSettings>
        where TConnSettings : MongodbSettings
    {
        private readonly TConnSettings mongoConnSetting;
        public MongoDbContext(TConnSettings mongoConnSetting)
        {

            this.mongoConnSetting = mongoConnSetting;
            MongoClientSettings mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(this.mongoConnSetting.ConnectionString));

            if (!string.IsNullOrWhiteSpace(this.mongoConnSetting.LoginDatabase))
            {
                mongoClientSettings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                var credential = MongoCredential.CreateCredential(this.mongoConnSetting.LoginDatabase, this.mongoConnSetting.UserName, this.mongoConnSetting.Password);
                mongoClientSettings.Credential = credential;
            }
            Client = new MongoClient(mongoClientSettings);
        }
        public MongoClient Client { get; }

        public IMongoDatabase GetDateBase()
        {
            return Client.GetDatabase(mongoConnSetting.Database, new MongoDatabaseSettings() { });
        }
    }

    public class MongoDefaultSetting : MongodbSettings {

    }
}
