using EPiServer.Commerce.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;

namespace FLS.CoffeeDesk.Infrastructure
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class SiteInitialization : IInitializableModule, IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            CatalogRouteHelper.MapDefaultHierarchialRouter(false);

            var enableCatalogRoot = context.Locate.Advanced.GetService<EnableCatalogRoot>();
            enableCatalogRoot.SetCatalogAccessRights();
            
            var defaultRolesAssign = context.Locate.Advanced.GetService<GrantDefaultRoles>();
            defaultRolesAssign.GrantTo("admin").GetAwaiter().GetResult();
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<EnableCatalogRoot>();
            context.Services.AddSingleton<GrantDefaultRoles>();
        }
    }
}
