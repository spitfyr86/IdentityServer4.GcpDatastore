using System.Collections.Generic;
using System.Linq;
using Spitfyr.GCP.Datastore.Adapter.Serialization;

namespace Spitfyr.IdentityServer4.GcpDS.Models
{
    public class IdentityResource : IdentityResourceBase
    {
        public IdentityResource()
        {
        }

        public IdentityResource(string name, string displayName, IEnumerable<string> claimTypes)
            : base(name, displayName, claimTypes)
        {
        }
    }
}