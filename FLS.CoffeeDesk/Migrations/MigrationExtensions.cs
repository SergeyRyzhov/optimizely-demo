using EPiServer.Commerce.Internal.Migration.Steps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FLS.CoffeeDesk.Migrations
{
    public static class MigrationExtensions
    {
        public static void AddMigrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<SiteSetupMigration>();
            services.AddTransient<UploadImagesMigration>();
            services.AddTransient<PagesSetupMigration>();
            services.AddTransient<CatalogSetupMigration>();

            services.AddTransient<IMigrationStep, CoffeeSiteSetup>();
        }
    }
}