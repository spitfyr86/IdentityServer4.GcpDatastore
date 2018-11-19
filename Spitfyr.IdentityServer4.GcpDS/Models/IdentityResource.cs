using System.Collections.Generic;
using System.Linq;
using Spitfyr.GCP.Datastore.Adapter.Serialization;

namespace Spitfyr.IdentityServer4.GcpDS.Models
{
    [Kind("IdentityResource")]
    public class IdentityResource : LongIdDatastoreEntity
    {
        public IdentityResource()
        {
        }

        public IdentityResource(string name, string displayName, IEnumerable<string> claimTypes)
        {
            Name = name;
            DisplayName = displayName;
            UserClaims = claimTypes.ToList();
        }

        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Emphasize { get; set; }
        public bool Enabled { get; set; } = true;
        //public ObjectId Id { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<string> UserClaims { get; set; }
    }
}