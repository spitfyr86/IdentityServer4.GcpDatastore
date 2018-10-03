using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Datastore.Adapter.Serialization;

namespace IdentityServer4.GcpDatastore.Models
{
    [Kind("ApiResource")]
    public class ApiResource : LongIdDatastoreEntity
    {
        public ApiResource()
        {
        }

        public ApiResource(string name, string displayName, IEnumerable<string> claimTypes)
        {
            Name = name;
            DisplayName = displayName;
            UserClaims = claimTypes.ToList();
        }

        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Enabled { get; set; } = true;
        //public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<ApiScope> Scopes { get; set; }
        public List<ApiSecret> Secrets { get; set; }
        public List<string> UserClaims { get; set; }
    }
}