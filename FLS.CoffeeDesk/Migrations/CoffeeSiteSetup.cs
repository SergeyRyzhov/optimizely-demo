using System;
using EPiServer.Commerce.Internal.Migration.Steps;
using EPiServer.Framework.Cache;
using Mediachase.Commerce.Shared;

namespace FLS.CoffeeDesk.Migrations
{
    internal class CoffeeSiteSetup : IMigrationStep
    {
        private readonly SiteSetupMigration _siteSetupMigration;

        private readonly PagesSetupMigration _pagesSetupMigration;
        private readonly CatalogSetupMigration _catalogSetupMigration;

        private readonly UploadImagesMigration _uploadImagesMigration;

        private readonly ISynchronizedObjectInstanceCache _cache;

        public CoffeeSiteSetup(SiteSetupMigration siteSetupMigration,
            PagesSetupMigration pagesSetupMigration,
            ISynchronizedObjectInstanceCache cache, CatalogSetupMigration catalogSetupMigration, UploadImagesMigration uploadImagesMigration)
        {
            _siteSetupMigration = siteSetupMigration;
            _pagesSetupMigration = pagesSetupMigration;
            _cache = cache;
            _catalogSetupMigration = catalogSetupMigration;
            _uploadImagesMigration = uploadImagesMigration;
        }

        public bool Execute(IProgressMessenger progressMessenger)
        {
            try
            {
                var site = _siteSetupMigration.SetupSite();
                var images = _uploadImagesMigration.UploadImages(site.GlobalAssetsRoot);
                var catalog = _catalogSetupMigration.SetupCatalog(images);
                var pages = _pagesSetupMigration.SetupPages(site, catalog, images);
                _siteSetupMigration.UpdateStartPage(ref site, pages.HomePage);

#pragma warning disable CS0618
                _cache.Clear();
#pragma warning restore CS0618
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public int Order => 5000;

        public string Name => "Create Coffee Desk Site";

        public string Description => "Create Coffee Desk Site basic configuration";
    }
}
