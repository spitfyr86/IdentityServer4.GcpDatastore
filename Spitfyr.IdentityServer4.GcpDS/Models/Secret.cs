using System;
using IdentityServer4;

namespace  Spitfyr.IdentityServer4.GcpDS.Models
{
    public abstract class Secret
    {
        //public ObjectId Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; } = IdentityServerConstants.SecretTypes.SharedSecret;
    }
}