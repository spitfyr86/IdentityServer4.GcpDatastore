using System;
using System.Collections.Generic;
using Spitfyr.GCP.Datastore.Adapter.Serialization;

namespace Spitfyr.IdentityServer4.GcpDS.Models
{
    public class ApiScope : DatastoreEntity
    {
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Emphasize { get; set; }
        //public ObjectId Id { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<string> UserClaims { get; set; }
    }
}