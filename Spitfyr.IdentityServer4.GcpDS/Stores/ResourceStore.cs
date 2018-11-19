using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Spitfyr.GCP.Datastore.Adapter;
using Spitfyr.IdentityServer4.GcpDS.DbContext;
using ApiResource = IdentityServer4.Models.ApiResource;
using IdentityResource = IdentityServer4.Models.IdentityResource;

namespace Spitfyr.IdentityServer4.GcpDS.Stores
{
    public class ResourceStore : IResourceStore
    {
        private readonly IConfigurationDatastoreDbContext _dbContext;

        public ResourceStore(IConfigurationDatastoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        Task<IEnumerable<ApiResource>> IResourceStore.FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return FindApiResourcesByScopeAsync(scopeNames);
        }

        Task<ApiResource> IResourceStore.FindApiResourceAsync(string name)
        {
            return FindApiResourceAsync(name);
        }

        Task<IEnumerable<IdentityResource>> IResourceStore.FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            return FindIdentityResourcesByScopeAsync(scopeNames);
        }

        public Task<Resources> GetAllResources()
        {
            return GetAllResourcesAsync();
        }

        /// <summary>
        /// Finds the API resource by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var filter = FilterBuilder<Models.ApiResource>.Equal(u => u.Name, name);
            var found = (await _dbContext.ApiResource.FindAsync(filter)).SingleOrDefault();

            return found.ToModel();
        }

        /// <summary>
        /// Gets API resources by scope name.
        /// </summary>
        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            scopeNames = scopeNames ?? new List<string>();

            //var records = (await _dbContext.ApiResource.FindInAsync(new Dictionary<string, dynamic>
            //{
            //    { "Scopes.Name", scopeNames.ToArray() }
            //})).ToList();

            var apiResources = await _dbContext.ApiResource.GetAllAsync();
            var records = apiResources.Where(x => scopeNames.Any(y => y == x.Name));

            return records.Select(Mapper.ToModel)
                .ToList();
        }

        /// <summary>
        /// Gets identity resources by scope name.
        /// </summary>
        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            scopeNames = scopeNames ?? new List<string>();

            //var records = (await _dbContext.IdentityResource.FindInAsync(new Dictionary<string, dynamic>
            //{
            //    { "Name", scopeNames.ToArray() }
            //})).ToList();

            var identityResources = await _dbContext.IdentityResource.GetAllAsync();
            var records = identityResources.Where(x => scopeNames.Any(y => y == x.Name));
            
            return records.Select(Mapper.ToModel)
                .ToList();
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        public async Task<Resources> GetAllResourcesAsync()
        {
            var apiResource = await _dbContext.ApiResource.GetAllAsync();
            var identityResource = await _dbContext.IdentityResource.GetAllAsync();
            
            return new Resources(identityResource.Select(Mapper.ToModel), apiResource.Select(Mapper.ToModel));
        }
    }
}