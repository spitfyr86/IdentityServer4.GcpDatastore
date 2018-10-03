using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.Adapter;
using IdentityServer4.GcpDatastore.DbContext;
using IdentityServer4.GcpDatastore.Models;
using IdentityServer4.Services;

namespace IdentityServer4.GcpDatastore
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IConfigurationDatastoreDbContext _dbContext;

        public CorsPolicyService(IConfigurationDatastoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            //var records = await _dbContext.Client.FindAsync(u => u.AllowedCorsOrigins.Contains(origin));

            var filter = FilterBuilder<Client>.Equal(u => u.AllowedCorsOrigins, origin);

            var records = await _dbContext.Client.FindAsync(filter);

            return records.FirstOrDefault() != null;
        }
    }
}