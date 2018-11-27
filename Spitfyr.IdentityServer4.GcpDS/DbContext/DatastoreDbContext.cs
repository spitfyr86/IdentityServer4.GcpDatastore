using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Datastore.V1;
using Grpc.Auth;
using Microsoft.Extensions.Options;
using Spitfyr.GCP.Datastore.Adapter;
using Spitfyr.IdentityServer4.GcpDS.Models;

namespace Spitfyr.IdentityServer4.GcpDS.DbContext
{
    public interface IConfigurationDatastoreDbContext
    {
        IDatastoreKind<ApiResource> ApiResource { get; }
        IDatastoreKind<Client> Client { get; }
        IDatastoreKind<IdentityResource> IdentityResource { get; }
    }

    public class ConfigurationDatastoreDbContext : IConfigurationDatastoreDbContext
    {
        public ConfigurationDatastoreDbContext(IDatastoreDatabase database, IOptions<ConfigurationDbOption> option)
        {
            Client = database.GetKind<Client>(option.Value.Client.Kind);
            ApiResource = database.GetKind<ApiResource>(option.Value.ApiResource.Kind);
            IdentityResource = database.GetKind<IdentityResource>(option.Value.IdentityResource.Kind);
        }

        public IDatastoreKind<ApiResource> ApiResource { get; }
        public IDatastoreKind<Client> Client { get; }
        public IDatastoreKind<IdentityResource> IdentityResource { get; }
    }


    public interface IOperationDbContext
    {
        IDatastoreKind<PersistedGrant> PersistedGrant { get; }
    }

    public class OperationDbContext : IOperationDbContext
    {
        public OperationDbContext(IDatastoreDatabase database, IOptions<OperationGcpDatastoreOption> option)
        {
            PersistedGrant = database.GetKind<PersistedGrant>(option.Value.PersistedGrant.Kind);
        }

        public IDatastoreKind<PersistedGrant> PersistedGrant { get; }
    }
}