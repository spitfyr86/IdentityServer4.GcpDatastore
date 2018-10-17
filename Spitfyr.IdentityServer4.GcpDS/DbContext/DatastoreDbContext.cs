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
        public ConfigurationDatastoreDbContext(IOptions<ConfigurationDbOption> option)
        {
            using (var stream = new FileStream(option.Value.CredentialsFilePath, FileMode.Open))
            {
                var googleCredential = GoogleCredential.FromStream(stream);
                var channel = new Grpc.Core.Channel(
                    DatastoreClient.DefaultEndpoint.Host,
                    googleCredential.ToChannelCredentials());

                var client = DatastoreClient.Create(channel);
                var datastoreDb = DatastoreDb.Create(option.Value.ProjectId, option.Value.Namespace, client);
                IDatastoreDatabase database = new DatastoreDatabase(datastoreDb);

                Client = database.GetKind<Client>(option.Value.Client.Kind);
                ApiResource = database.GetKind<ApiResource>(option.Value.ApiResource.Kind);
                IdentityResource = database.GetKind<IdentityResource>(option.Value.IdentityResource.Kind);
            }
        }

        public IDatastoreKind<ApiResource> ApiResource { get; private set; }
        public IDatastoreKind<Client> Client { get; private set; }
        public IDatastoreKind<IdentityResource> IdentityResource { get; private set; }
    }


    public interface IOperationDbContext
    {
        IDatastoreKind<PersistedGrant> PersistedGrant { get; }
    }

    public class OperationDbContext : IOperationDbContext
    {
        public OperationDbContext(IOptions<OperationGcpDatastoreOption> option)
        {
            using (var stream = new FileStream(option.Value.CredentialsFilePath, FileMode.Open))
            {
                var googleCredential = GoogleCredential.FromStream(stream);
                var channel = new Grpc.Core.Channel(
                    DatastoreClient.DefaultEndpoint.Host,
                    googleCredential.ToChannelCredentials());

                var client = DatastoreClient.Create(channel);
                var datastoreDb = DatastoreDb.Create(option.Value.ProjectId, option.Value.Namespace, client);
                IDatastoreDatabase database = new DatastoreDatabase(datastoreDb);

                PersistedGrant = database.GetKind<PersistedGrant>(option.Value.PersistedGrant.Kind);
            }
        }

        public IDatastoreKind<PersistedGrant> PersistedGrant { get; private set; }
    }
}