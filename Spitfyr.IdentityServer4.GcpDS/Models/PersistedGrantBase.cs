using System;
using Spitfyr.GCP.Datastore.Adapter.Serialization;

namespace  Spitfyr.IdentityServer4.GcpDS.Models
{
    [Kind("PersistedGrant")]
    public class PersistedGrantBase : LongIdDatastoreEntity
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