using System;
using IdentityServer4.GcpDatastore.DbContext;
using IdentityServer4.GcpDatastore.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer4.GcpDatastore
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddConfigurationStore(this IIdentityServerBuilder builder,
            Action<ConfigurationDbOption> storeOptionsAction = null)
        {
            // var options = new ConfigurationDBOption();
            // storeOptionsAction?.Invoke(options);
            // builder.Services.AddSingleton(options);
            builder.Services.Configure<ConfigurationDbOption>(storeOptionsAction);

            builder.Services.AddTransient<IConfigurationDatastoreDbContext, ConfigurationDatastoreDbContext>();

            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            return builder;
        }

        public static IIdentityServerBuilder AddOperationalStore(
            this IIdentityServerBuilder builder,
            Action<OperationGcpDatastoreOption> storeOptionsAction = null)
        {
            // var options = new OperationGcpDatastoreOption();
            // storeOptionsAction?.Invoke(options);
            builder.Services.Configure<OperationGcpDatastoreOption>(storeOptionsAction);
            // builder.Services.AddSingleton(options);

            builder.Services.AddTransient<IOperationDbContext, OperationDbContext>();

            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            builder.Services.AddSingleton<TokenCleanup>();
            builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();

            return builder;
        }
    }
}