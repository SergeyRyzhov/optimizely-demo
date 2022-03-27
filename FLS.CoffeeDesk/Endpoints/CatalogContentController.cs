using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Web.Mvc;
using FLS.CoffeeDesk.Content;
using FLS.CoffeeDesk.Content.Metadata;
using FLS.CoffeeDesk.Models;
using Mediachase.Commerce.Pricing;
using Microsoft.AspNetCore.Mvc;

namespace FLS.CoffeeDesk.Endpoints
{
    public class CatalogContentController : ContentController<CatalogContent>
    {
        private readonly IContentRepository _contentRepository;

        private readonly IContentLoader _contentLoader;

        private IRelationRepository _relationRepository;

        private readonly IPriceService _priceService;

        public CatalogContentController(IContentRepository contentRepository, IContentLoader contentLoader, IRelationRepository relationRepository, IPriceService priceService)
        {
            _contentRepository = contentRepository;
            _contentLoader = contentLoader;
            _relationRepository = relationRepository;
            _priceService = priceService;
        }

        [HttpGet]
        public IActionResult Index(CatalogContent catalogContent)
        {
            var vm = CreateCatalogViewModel(catalogContent);
            return View(vm);
        }

        private CatalogViewModel CreateCatalogViewModel(CatalogContent catalogContent)
        {
            var categories = _contentLoader.GetChildren<DrinksCategory>(catalogContent.ContentLink).ToArray();
            var productsMap = categories.ToDictionary(c => c.ContentLink.ID, GetCategoryProducts);
            var variationsMap = productsMap.SelectMany(m=>m.Value).ToDictionary(c => c.ContentLink.ID, GetProductVariations);

            return new CatalogViewModel(_priceService, new BeansOriginSelectionFactory())
            {
                Catalog = catalogContent,
                Categories = categories,
                Products = productsMap,
                Variations = variationsMap
            };
        }

        private CoffeeProduct[] GetCategoryProducts(DrinksCategory category)
        {
            return _contentLoader.GetChildren<CoffeeProduct>(category.ContentLink).ToArray();
        }
        
        private CoffeeVariation[] GetProductVariations(CoffeeProduct product)
        {
            var relations = _relationRepository.GetChildren<Relation>(product.ContentLink);
            var variations = relations.Select(r => _contentRepository.Get<CoffeeVariation>(r.Child));
            return variations.ToArray();
        }
    }
}