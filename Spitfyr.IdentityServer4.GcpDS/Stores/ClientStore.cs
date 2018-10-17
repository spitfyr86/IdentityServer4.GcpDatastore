using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Spitfyr.GCP.Datastore.Adapter;
using Spitfyr.IdentityServer4.GcpDS.DbContext;

namespace Spitfyr.IdentityServer4.GcpDS.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly IConfigurationDatastoreDbContext _dbContext;

        public ClientStore(IConfigurationDatastoreDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            //var filter = Builders<Models.Client>.Filter.Eq(u => u.ClientId, clientId);
            var filter = FilterBuilder<Models.Client>.Equal(u => u.ClientId, clientId);

            var client = (await _dbContext.Client.FindAsync(filter)).SingleOrDefault();

            return client.ToModel();
        }
    }
}