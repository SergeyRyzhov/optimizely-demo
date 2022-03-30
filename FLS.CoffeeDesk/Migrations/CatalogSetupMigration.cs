using System;
using System.IO;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Shell.ObjectEditing;
using FLS.CoffeeDesk.Content;
using FLS.CoffeeDesk.Content.Metadata;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;

namespace FLS.CoffeeDesk.Migrations
{
    internal class CatalogSetupMigration
    {
        private readonly IContentRepository _contentRepository;

        private readonly ReferenceConverter _referenceConverter;

        private readonly IPriceService _priceService;

        private readonly Random _rnd;

        public CatalogSetupMigration(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IPriceService priceService)
        {
            _contentRepository = contentRepository;
            _referenceConverter = referenceConverter;
            _priceService = priceService;
            _rnd = new Random();
        }

        public CatalogContent SetupCatalog(ImageFile[] imageFiles)
        {
            var catalog = CreateCatalogContent();

            var category = CreateDrinksCategory(catalog);

            foreach (var imageFile in imageFiles.Where(i => !i.Name.StartsWith("main")))
            {
                var product = CreateCoffeeProduct(category, imageFile);

                for (int i = 0; i < 4; i++)
                {
                    var item = new BeansOriginSelectionFactory().GetSelections(null).ToArray()[i];
                    var beansRoasting = Faker.Enum.Random<BeansRoasting>();

                    var variation = CreateCoffeeVariation(product, beansRoasting, item, imageFile);
                    
                    AddPrice(variation);
                }
            }

            return catalog;
        }

        private DrinksCategory CreateDrinksCategory(CatalogContent catalog)
        {
            var category = _contentRepository.GetDefault<DrinksCategory>(catalog.ContentLink);
            category.Name = "Coffee category";
            _contentRepository.Save(category, SaveAction.Publish, AccessLevel.NoAccess);
            return category;
        }

        private CatalogContent CreateCatalogContent()
        {
            var catalog = _contentRepository.GetDefault<CatalogContent>(_referenceConverter.GetRootLink());
            catalog.Name = "Best Coffee Drinks Catalog";
            catalog.DefaultCurrency = "CZK";
            catalog.WeightBase = "KG";
            catalog.LengthBase = "CM";
            catalog.DefaultLanguage = "en";
            _contentRepository.Save(catalog, SaveAction.Publish, AccessLevel.NoAccess);
            return catalog;
        }

        private void AddPrice(CoffeeVariation variation)
        {
            _priceService.SetCatalogEntryPrices(new CatalogKey(variation.Code), new IPriceValue[]
            {
                new PriceValue()
                {
                    CatalogKey = new CatalogKey(variation.Code),
                    MarketId = MarketId.Default,
                    MinQuantity = 0,
                    UnitPrice = new Money(_rnd.Next(150), Currency.CZK),
                    ValidFrom = DateTime.Now.AddYears(-1),
                    ValidUntil = DateTime.Now.AddYears(10),
                    CustomerPricing = new CustomerPricing(CustomerPricing.PriceType.AllCustomers, "CZK_01")
                }
            });
        }

        private CoffeeVariation CreateCoffeeVariation(CoffeeProduct product, BeansRoasting beansRoasting, ISelectItem item,
            ImageFile imageFile)
        {
            var variation = _contentRepository.GetDefault<CoffeeVariation>(product.ContentLink);
            variation.Strength = (int)beansRoasting;
            variation.BeansOrigin = item.Value.ToString();
            variation.Name = $"{beansRoasting.ToString()} {item.Text}";
            variation.Image = imageFile.ContentLink;
            variation.CommerceMediaCollection.Add(new CommerceMedia(imageFile.ContentLink, "default", "default", 0));
            _contentRepository.Save(variation, SaveAction.Publish, AccessLevel.NoAccess);
            return variation;
        }

        private CoffeeProduct CreateCoffeeProduct(DrinksCategory category, ImageFile imageFile)
        {
            var product = _contentRepository.GetDefault<CoffeeProduct>(category.ContentLink);
            product.Name = Path.GetFileNameWithoutExtension(imageFile.Name);
            product.Description = new XhtmlString(GenerateSomeHtml());
            product.Image = imageFile.ContentLink;
            product.CommerceMediaCollection.Add(new CommerceMedia(imageFile.ContentLink, "default", "default", 0));
            _contentRepository.Save(product, SaveAction.Publish, AccessLevel.NoAccess);
            return product;
        }

        private string GenerateSomeHtml()
        {
            return $"<h5>Description</h5><p>{Faker.Lorem.Sentence()}</p>";
        }
    }
}