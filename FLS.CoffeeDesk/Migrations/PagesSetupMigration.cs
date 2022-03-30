using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.Web;
using FLS.CoffeeDesk.Content;

namespace FLS.CoffeeDesk.Migrations
{
    internal class PagesSetupMigration
    {
        private readonly IContentRepository _contentRepository;

        private readonly ContentAssetHelper _contentAssetHelper;

        private readonly Random _rnd;

        public PagesSetupMigration(IContentRepository contentRepository, ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _contentAssetHelper = contentAssetHelper;
            _rnd = new Random();
        }

        public SitePages SetupPages(SiteDefinition siteDefinition, CatalogContent catalog, ImageFile[] imageFiles)
        {
            var startPage = siteDefinition.StartPage;
            if (_contentRepository.Get<PageData>(startPage) is HomePage homePage)
            {
                return new SitePages
                {
                    HomePage = homePage
                };
            }

            var mainImages = imageFiles.Where(i => i.Name.StartsWith("main")).ToArray();
            var randomCatalogImage = imageFiles.Where(i => !i.Name.StartsWith("main")).ToArray()[0];
            homePage = CreateHomePage1(catalog, mainImages, randomCatalogImage);

            return new SitePages
            {
                HomePage = homePage
            };
        }

        private HomePage CreateHomePage1(CatalogContent catalog, ImageFile[] mainImages, ImageFile randomCatalogImage)
        {
            var homePage = _contentRepository.GetDefault<HomePage>(ContentReference.RootPage);
            homePage.Name = "Home page";
            _contentRepository.Publish(homePage, AccessLevel.NoAccess);

            var homeAssets = _contentAssetHelper.GetOrCreateAssetFolder(homePage.ContentLink);
            var introBlock = CreateCatalogIntroBlock(catalog, randomCatalogImage, homeAssets);

            IList<ContentAreaItem> blocks = new List<ContentAreaItem>
            {
                new()
                {
                    ContentLink = introBlock.ContentLink
                }
            };

            foreach (var imageFile in mainImages)
            {
                var textBlock = CreateTextBlock(homeAssets, imageFile);

                blocks.Add(new ContentAreaItem
                {
                    ContentLink = textBlock.ContentLink
                });
            }

            homePage = homePage.CreateWritableClone() as HomePage;
            homePage.Content = new ContentArea();
            foreach (var block in blocks.OrderBy(x => _rnd.Next()).ToArray())
            {
                homePage.Content.Items.Add(block);
            }

            _contentRepository.Publish(homePage, AccessLevel.NoAccess);
            return homePage;
        }

        private IContent CreateTextBlock(ContentAssetFolder homeAssets, ImageFile imageFile)
        {
            var textContent = _contentRepository.GetDefault<TextBlock>(homeAssets.ContentLink);
            textContent.Content = new XhtmlString(GenerateSomeHtml());
            textContent.Image = imageFile.ContentLink;
            var textBlock = textContent as IContent;
            textBlock.Name = $"Text bloc {imageFile.Name}";
            _contentRepository.Publish(textBlock, AccessLevel.NoAccess);
            return textBlock;
        }

        private IContent CreateCatalogIntroBlock(CatalogContent catalog, ImageFile randomCatalogImage,
            ContentAssetFolder homeAssets)
        {
            var catalogIntro = _contentRepository.GetDefault<CatalogIntroBlock>(homeAssets.ContentLink);
            catalogIntro.Catalog = catalog.ContentLink;
            catalogIntro.Description = new XhtmlString(GenerateSomeHtml());
            catalogIntro.Image = randomCatalogImage.ContentLink;
            var introBlock = catalogIntro as IContent;
            introBlock.Name = "Catalog intro";
            _contentRepository.Publish(introBlock, AccessLevel.NoAccess);
            return introBlock;
        }

        public class SitePages
        {
            public HomePage HomePage { get; set; }
        }

        private string GenerateSomeHtml()
        {
            return $"<h2>{Faker.Company.Name()}</h2><p>{Faker.Lorem.Paragraph()}</p>";
        }
    }
}