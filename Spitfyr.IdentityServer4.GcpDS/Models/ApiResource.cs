using System.Collections.Generic;
using System.Linq;
using Spitfyr.GCP.Datastore.Adapter.Serialization;

namespace Spitfyr.IdentityServer4.GcpDS.Models
{
    public class ApiResource : ApiResourceBase
    {
        public ApiResource()
        {
        }

        public ApiResource(string name, string displayName, IEnumerable<string> claimTypes)
            : base(name, displayName, claimTypes)
        {
        }
    }
}