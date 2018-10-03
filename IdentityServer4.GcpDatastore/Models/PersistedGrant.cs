using System;
using Google.Cloud.Datastore.Adapter.Serialization;

namespace  IdentityServer4.GcpDatastore.Models
{
    [Kind("PersistedGrant")]
    public class PersistedGrant : LongIdDatastoreEntity
    {
        //public ObjectId Id { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public string SubjectId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Expiration { get; set; }
        public string Data { get; set; }
    }
}