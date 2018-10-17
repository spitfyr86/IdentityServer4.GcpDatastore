using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Spitfyr.GCP.Datastore.Adapter;
using Spitfyr.IdentityServer4.GcpDS.DbContext;
using Spitfyr.IdentityServer4.GcpDS.Models;

namespace Spitfyr.IdentityServer4.GcpDS
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