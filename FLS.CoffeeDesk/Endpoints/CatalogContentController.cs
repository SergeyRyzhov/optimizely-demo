using System.Linq;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Web.Mvc;
using FLS.CoffeeDesk.Content;
using FLS.CoffeeDesk.Models;
using Microsoft.AspNetCore.Mvc;

namespace FLS.CoffeeDesk.Endpoints
{
    public class CatalogContentController : ContentController<CatalogContent>
    {
        private readonly IContentRepository _contentRepository;

        private readonly IContentLoader _contentLoader;

        public CatalogContentController(IContentRepository contentRepository, IContentLoader contentLoader)
        {
            _contentRepository = contentRepository;
            _contentLoader = contentLoader;
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

            return new CatalogViewModel
            {
                Catalog = catalogContent,
                Categories = categories,
                Products = productsMap
            };
        }

        private CoffeeProduct[] GetCategoryProducts(DrinksCategory category)
        {
            return _contentLoader.GetChildren<CoffeeProduct>(category.ContentLink).ToArray();
        }
    }
}