﻿namespace Spitfyr.IdentityServer4.GcpDS.Models
{
    public class ClientClaim
    {
        public ClientClaim()
        {

        }
        public ClientClaim(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; set; }
        public string Value { get; set; }
    }
}