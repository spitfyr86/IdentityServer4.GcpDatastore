using System;
using System.Linq;
using IdentityServer4.GcpDatastore;
using IdentityServer4.GcpDatastore.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace Sample
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IConfigurationDatastoreDbContext>();

                //var count = context.Client.Count(Builders<IdentityServer4.MongoDB.Models.Client>.Filter.Empty);

                //TODO: Change this to only count records on query
                var count = context.Client.GetAll().Count();

                if (count == 0)
                {
                    foreach (var client in Config.GetClients().ToList())
                    {
                        context.Client.InsertOne(client.ToEntity());
                    }
                }

                //count = context.IdentityResource.Count(Builders<IdentityServer4.MongoDB.Models.IdentityResource>.Filter.Empty);
                count = context.IdentityResource.GetAll().Count();

                if (count == 0)
                {
                    foreach (var resource in Config.GetIdentityResources().ToList())
                    {
                        context.IdentityResource.InsertOne(resource.ToEntity());
                    }
                }

                //count = context.ApiResource.Count(Builders<IdentityServer4.MongoDB.Models.ApiResource>.Filter.Empty);
                count = context.ApiResource.GetAll().Count();

                if (count == 0)
                {
                    foreach (var resource in Config.GetApiResources().ToList())
                    {
                        context.ApiResource.InsertOne(resource.ToEntity());
                    }
                }
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
    }
}
