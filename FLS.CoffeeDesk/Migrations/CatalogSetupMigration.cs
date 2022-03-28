using System;
using System.IO;
using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
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

        private string GenerateSomeHtml()
        {
            return $"<h5>Description</h5><p>{Faker.Lorem.Sentence()}</p>";
        }

        public CatalogContent SetupCatalog(ImageFile[] imageFiles)
        {
            var catalog = _contentRepository.GetDefault<CatalogContent>(_referenceConverter.GetRootLink());
            catalog.Name = "Best Coffee Drinks Catalog";
            catalog.DefaultCurrency = "CZK";
            catalog.WeightBase = "KG";
            catalog.LengthBase = "CM";
            catalog.DefaultLanguage = "en";
            _contentRepository.Save(catalog, SaveAction.Publish, AccessLevel.NoAccess);

            var category = _contentRepository.GetDefault<DrinksCategory>(catalog.ContentLink);
            category.Name = "Coffee category";
            _contentRepository.Save(category, SaveAction.Publish, AccessLevel.NoAccess);

            foreach (var imageFile in imageFiles.Where(i => !i.Name.StartsWith("main")).ToArray())
            {
                var product = _contentRepository.GetDefault<CoffeeProduct>(category.ContentLink);
                product.Name = Path.GetFileNameWithoutExtension(imageFile.Name);
                product.Description = new XhtmlString(GenerateSomeHtml());
                product.Image = imageFile.ContentLink;
                _contentRepository.Save(product, SaveAction.Publish, AccessLevel.NoAccess);

                for (int i = 0; i < 4; i++)
                {
                    var item = new BeansOriginSelectionFactory().GetSelections(null).ToArray()[i];
                    var beansRoasting = Faker.Enum.Random<BeansRoasting>();

                    var variation = _contentRepository.GetDefault<CoffeeVariation>(product.ContentLink);
                    variation.Strength = (int)beansRoasting;
                    variation.BeansOrigin = item.Value.ToString();
                    variation.Name = $"{beansRoasting.ToString()} {item.Text}";
                    variation.Image = imageFile.ContentLink;
                    _contentRepository.Save(variation, SaveAction.Publish, AccessLevel.NoAccess);
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
            }

            return catalog;
        }
    }
}