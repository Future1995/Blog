using HwaTec.Blog.IRep;
using HwaTec.Blog.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HwaTec.Blog.MongoRep
{
    public class NoSqlBaseRepository<TEntity>
       where TEntity : MongoEntityBase
    {
        public IMongoCollection<TEntity> Collection;

        public IQueryable<TEntity> Table => throw new NotImplementedException();

        public NoSqlBaseRepository(MongoDbContext<MongoDefaultSetting> mongoDatabase)
        {
            var mongoDatabase1 = mongoDatabase.GetDateBase();
            Collection = mongoDatabase1.GetCollection<TEntity>(typeof(TEntity).Name, new MongoCollectionSettings() { });
        }

        public TEntity Add(TEntity entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public async Task AddAsync(TEntity entity, InsertOneOptions options = null)
        {
            await Collection.InsertOneAsync(entity, options: options);
        }

        public async Task AddManyAsync(List<TEntity> items)
        {
            await Collection.InsertManyAsync(items);
        }

        public async Task<long> UpdateManyAsync(Expression<Func<TEntity, bool>> expression, UpdateDefinition<TEntity> updateDefinition)
        {
            var result = await Collection.UpdateManyAsync<TEntity>(expression, updateDefinition);
            return result.ModifiedCount;
        }

        public async Task<long> UpdateManyAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> updateDefinition)
        {
            var result = await Collection.UpdateManyAsync(filter, updateDefinition);
            return result.ModifiedCount;
        }

        public bool Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Sysid, entity.Sysid);
            return Collection.ReplaceOne(filter, entity).ModifiedCount == 1;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Sysid, entity.Sysid);
            var result = await Collection.ReplaceOneAsync(filter, entity);
            return result.ModifiedCount == 1;
        }

        public async Task<bool> ReplaceOneAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Sysid, entity.Sysid);
            return (await Collection.ReplaceOneAsync(filter, entity)).ModifiedCount == 1;
        }

        public async Task<TEntity> GetByPropertyAsync<TField>(Expression<Func<TEntity, TField>> expression, TField value)
        {
            var filter = Builders<TEntity>.Filter.Eq(expression, value);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
        public  TEntity GetByProperty<TField>(Expression<Func<TEntity, TField>> expression, TField value)
        {
            var filter = Builders<TEntity>.Filter.Eq(expression, value);
            return  Collection.Find(filter).FirstOrDefault();
        }

        public bool Delete(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Sysid, entity.Sysid);
            return Collection.DeleteOne(filter).DeletedCount > 0;
        }

        public async Task DeleteManyAsync(FilterDefinition<TEntity> filter)
        {
            await Collection.DeleteManyAsync(filter);
        }

        public async Task DeleteAllAsync()
        {
            await Collection.DeleteManyAsync(Builders<TEntity>.Filter.Empty);
        }

        public IList<TEntity> Search(Expression<Func<TEntity, bool>> predicate, SortDefinition<TEntity> sort)
        {
            return Collection.AsQueryable<TEntity>().Where(predicate.Compile()).ToList();
        }

        public async Task<IList<TEntity>> SearchAsync(FilterDefinition<TEntity> filter)
        {
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IList<TEntity>> SearchAsync(FilterDefinition<TEntity> filter, SortDefinition<TEntity> sort)
        {
            return await Collection.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<IList<TEntity>> SearchAsync(FilterDefinition<TEntity> filter, SortDefinition<TEntity> sort, int pageCount)
        {
            return await Collection.Find(filter).Sort(sort).Limit(pageCount).ToListAsync();
        }

        public async Task<IList<TEntity>> SearchAsync(FilterDefinition<TEntity> filter, int pageCount)
        {
            return await Collection.Find(filter).Limit(pageCount).ToListAsync();
        }

        public async Task<TEntity> SearchOneAsync(FilterDefinition<TEntity> filter)
        {
            return await Collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<Page<TEntity>> PaginationSearchAsync(FilterDefinition<TEntity> filter, SortDefinition<TEntity> sort, int pageIndex = 1, int pageSize = 20, bool ignoreCount = true, int defaultCountNumber = 50000)
        {
            var total = ignoreCount ? defaultCountNumber : Collection.Find(filter).CountAsync().Result;
            var records = Collection.Find(filter).Sort(sort).Limit(pageSize).Skip((pageIndex - 1) * pageSize).ToListAsync();
            return await Task.FromResult(new Page<TEntity>()
            {
                Records = records.Result,
                Paging = new Paging() { Total = (int)total, PageIndex = pageIndex, PageSize = pageSize }
            });
        }

        public IList<TEntity> GetAll()
        {
            var filter = Builders<TEntity>.Filter.Empty;
            return Collection.Find(filter).ToList();
        }

        public TEntity GetById(object id)
        {
            var guid = (Guid)id;
            var filter = Builders<TEntity>.Filter.Eq(x=>x.Sysid, guid);
            return  Collection.Find(filter).FirstOrDefault();
        }

        public IQueryable<TEntity> LoadEntities(Expression<Func<TEntity, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, S>> orderbyLambda, bool isAsc)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntities(object [] ids)
        {
            var list = new List<WriteModel<TEntity>>();

            foreach (var id in ids)
            {
                var filter = Builders<TEntity>.Filter.Eq(x => x.Sysid, id);
                list.Add(new DeleteOneModel<TEntity>(filter));
            }
            Collection.BulkWriteAsync(list).Wait();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
