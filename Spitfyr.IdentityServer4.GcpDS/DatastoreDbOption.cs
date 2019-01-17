namespace Spitfyr.IdentityServer4.GcpDS
{
    public abstract class DatastoreDbOption
    {
        public string CredentialsFilePath { get; set; }
        public string ProjectId { get; set; }
        public string Namespace { get; set; }
        public string EntityPrefix { get; set; }
    }

    public class ConfigurationDbOption : DatastoreDbOption
    {
        public Option ApiResource { get; set; } = new Option
        {
            Kind = "ApiResource",
        };

        public Option Client { get; set; } = new Option
        {
            Kind = "Client",
        };

        public Option IdentityResource { get; set; } = new Option
        {
            Kind = "IdentityResource",
        };
    }

    public class OperationGcpDatastoreOption : DatastoreDbOption
    {
        public Option PersistedGrant { get; set; } = new Option
        {
            Kind = "PersistedGrant",
        };
        public bool EnableTokenCleanup { get; set; } = true;
        public int TokenCleanupInterval { get; set; } = 3600;
        // public int TokenCleanupBatchSize { get; set; } = 100;
    }

    public class Option
    {
        public string Kind { get; set; }
        public bool ManageIndicies { get; set; } = true;
    }
}