using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.Adapter;
using IdentityServer4.GcpDatastore.DbContext;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer4.GcpDatastore.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IOperationDbContext _dbContext;

        public PersistedGrantStore(IOperationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            //var filter = Builders<Models.PersistedGrant>.Filter.Eq(u => u.SubjectId, subjectId);
            var filter = FilterBuilder<Models.PersistedGrant>.Equal(u => u.SubjectId, subjectId);

            var records = (await _dbContext.PersistedGrant.FindAsync(filter)).ToList();

            return records.Select(Map).ToList();
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            //var filter = Builders<Models.PersistedGrant>.Filter.Eq(u => u.Key, key);
            var filter = FilterBuilder<Models.PersistedGrant>.Equal(u => u.Key, key);

            var record = (await _dbContext.PersistedGrant.FindAsync(filter)).FirstOrDefault();

            return Map(record);
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            //var filter = Builders<Models.PersistedGrant>.Filter.And(
            //        Builders<Models.PersistedGrant>.Filter.Eq(u => u.SubjectId, subjectId),
            //        Builders<Models.PersistedGrant>.Filter.Eq(u => u.ClientId, clientId)
            //);

            var filter = FilterBuilder<Models.PersistedGrant>.And(
                    FilterBuilder<Models.PersistedGrant>.Equal(u => u.SubjectId, subjectId),
                    FilterBuilder<Models.PersistedGrant>.Equal(u => u.ClientId, clientId)
                );

            return _dbContext.PersistedGrant.DeleteManyAsync(filter);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            //var filter = Builders<Models.PersistedGrant>.Filter.And(
            //        Builders<Models.PersistedGrant>.Filter.Eq(u => u.SubjectId, subjectId),
            //        Builders<Models.PersistedGrant>.Filter.Eq(u => u.ClientId, clientId),
            //        Builders<Models.PersistedGrant>.Filter.Eq(u => u.Type, type)
            //);

            var filter = FilterBuilder<Models.PersistedGrant>.And(
                FilterBuilder<Models.PersistedGrant>.Equal(u => u.SubjectId, subjectId),
                FilterBuilder<Models.PersistedGrant>.Equal(u => u.ClientId, clientId),
                FilterBuilder<Models.PersistedGrant>.Equal(u => u.Type, type)
            );

            return _dbContext.PersistedGrant.DeleteManyAsync(filter);
        }

        public Task RemoveAsync(string key)
        {
            //var filter = Builders<Models.PersistedGrant>.Filter.Eq(u => u.Key, key);
            var filter = FilterBuilder<Models.PersistedGrant>.Equal(u => u.Key, key);

            return _dbContext.PersistedGrant.DeleteManyAsync(filter);
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            //var filter = Builders<Models.PersistedGrant>.Filter.Eq(u => u.Key, grant.Key);
            var filter = FilterBuilder<Models.PersistedGrant>.Equal(u => u.Key, grant.Key);

            var existing = (await _dbContext.PersistedGrant.FindAsync(filter)).SingleOrDefault();
            if (existing == null)
            {
                await _dbContext.PersistedGrant.InsertOneAsync(Map(null, grant));
            }
            else
            {
                await _dbContext.PersistedGrant.UpdateAsync(Map(existing, grant));
            }
        }

        private PersistedGrant Map(Models.PersistedGrant grant)
        {
            return new PersistedGrant
            {
                ClientId = grant.ClientId,
                CreationTime = grant.CreationTime,
                Data = grant.Data,
                Expiration = grant.Expiration,
                Key = grant.Key,
                SubjectId = grant.SubjectId,
                Type = grant.Type
            };
        }

        private Models.PersistedGrant Map(Models.PersistedGrant existing, PersistedGrant grant)
        {
            if (existing == null)
                existing = new Models.PersistedGrant();
            
            existing.ClientId = grant.ClientId;
            existing.CreationTime = grant.CreationTime;
            existing.Data = grant.Data;
            existing.Expiration = grant.Expiration;
            existing.Key = grant.Key;
            existing.SubjectId = grant.SubjectId;
            existing.Type = grant.Type;

            return existing;
        }
    }
}