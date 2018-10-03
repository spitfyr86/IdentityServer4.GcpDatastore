using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.Adapter;
using Google.Cloud.Datastore.V1;
using IdentityServer4.GcpDatastore.DbContext;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer4.GcpDatastore.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IConfigurationDatastoreDbContext _dbContext;

        public ResourceStore(IConfigurationDatastoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Finds the API resource by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            //var filter = Builders<Models.ApiResource>.Filter.Eq(u => u.Name, name);
            var filter = FilterBuilder<Models.ApiResource>.Equal(u => u.Name, name);

            var found = (await _dbContext.ApiResource.FindAsync(filter)).SingleOrDefault();

            return found.ToModel();
        }

        /// <summary>
        /// Gets API resources by scope name.
        /// </summary>
        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();
            /*
            var apis =
                from api in _dbContext.ApiResource.AsQueryable()
                where api.Scopes.Where(x => names.Contains(x.Name)).Any()
                select api;
             or this?   

            var filter = Builders<ApiResource>.Filter.Where(p => p.Scopes.Any(b => scopeNames.Contains(b.Name)));
             */

            //var records = await _dbContext.ApiResource
            //    .FindAsync(u => u.Scopes.Any(s => names.Contains(s.Name)))

            //TODO: Check if this works
            var filter = Filter.Equal("Scopes.Name", names);

            var records = (await _dbContext.ApiResource.FindAsync(filter)).ToList();

            return records.Select(Mapper.ToModel)
                .ToList();
        }

        /// <summary>
        /// Gets identity resources by scope name.
        /// </summary>
        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            //var filter = Builders<Models.IdentityResource>.Filter.In(p => p.Name, scopeNames);
            //TODO: Check if this works
            var filter = Filter.Equal("Scopes.Name", scopeNames.ToArray());

            var records = (await _dbContext.IdentityResource.FindAsync(filter)).ToList();

            return records.Select(Mapper.ToModel)
                .ToList();
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        public async Task<Resources> GetAllResourcesAsync()
        {
            //var allApiFilter = Builders<Models.ApiResource>.Filter.Empty;
            //var allIndenityFilter = Builders<Models.IdentityResource>.Filter.Empty;

            //var apiResource = await _dbContext.ApiResource.Find(allApiFilter).ToListAsync();
            //var identityResource = await _dbContext.IdentityResource.Find(allIndenityFilter).ToListAsync();

            var apiResource = await _dbContext.ApiResource.GetAllAsync();
            var identityResource = await _dbContext.IdentityResource.GetAllAsync();

            return new Resources(identityResource.Select(Mapper.ToModel), apiResource.Select(Mapper.ToModel));
        }
    }
}