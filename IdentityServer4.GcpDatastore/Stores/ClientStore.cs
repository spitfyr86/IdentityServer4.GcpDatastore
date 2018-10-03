using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.Adapter;
using IdentityServer4.GcpDatastore.DbContext;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer4.GcpDatastore.Stores
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