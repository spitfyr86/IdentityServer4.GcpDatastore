// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection;
using Spitfyr.IdentityServer4.GcpDS;
using Spitfyr.IdentityServer4.GcpDS.DbContext;

namespace QuickstartIdentityServer
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            /*var connectionString = @"server=(localdb)\mssqllocaldb;database=IdentityServer4.QuickStart.EntityFramework;trusted_connection=yes";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;*/

            // configure identity server with in-memory users, but EF stores for clients and scopes
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddTestUsers(Config.GetUsers())
                // support for Google Cloud Datastore
                .AddConfigurationStore(option =>
                {
                    option.ProjectId = "[YOUR-PROJECT-ID]";
                    option.Namespace = "[YOUR-NAMESPACE]";
                    option.CredentialsFilePath = ".[YOUR-PROJECT-SERVICE-KEY-FILEPATH]";
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(option =>
                {
                    option.ProjectId = "[YOUR-PROJECT-ID]";
                    option.Namespace = "[YOUR-NAMESPACE]";
                    option.CredentialsFilePath = ".[YOUR-PROJECT-SERVICE-KEY-FILEPATH]";

                    // this enables automatic token cleanup. this is optional.
                    option.EnableTokenCleanup = true;
                    option.TokenCleanupInterval = 5 * 60;
                });

            /*.AddConfigurationStore(builder =>
                builder.UseSqlServer(connectionString, options =>
                    options.MigrationsAssembly(migrationsAssembly)))
            .AddOperationalStore(builder =>
                builder.UseSqlServer(connectionString, options =>
                    options.MigrationsAssembly(migrationsAssembly)));*/
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // this will do the initial DB population
            InitializeDatabase(app);

            loggerFactory.AddConsole(LogLevel.Debug);
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                DisplayName = "Google",
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

                ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com",
                ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo"
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IConfigurationDatastoreDbContext>();

                var count = context.Client.GetAll().Count();
                if (count == 0)
                {
                    foreach (var client in Config.GetClients().ToList())
                    {
                        context.Client.InsertOne(client.ToEntity());
                    }
                }

                count = context.IdentityResource.GetAll().Count();
                if (count == 0)
                {
                    foreach (var resource in Config.GetIdentityResources().ToList())
                    {
                        context.IdentityResource.InsertOne(resource.ToEntity());
                    }
                }

                count = context.ApiResource.GetAll().Count();
                if (count == 0)
                {
                    foreach (var resource in Config.GetApiResources().ToList())
                    {
                        context.ApiResource.InsertOne(resource.ToEntity());
                    }
                }
            }
        }
    }
}