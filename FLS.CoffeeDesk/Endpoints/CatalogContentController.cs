using System.Globalization;
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
            var categories = _contentRepository.GetChildren<DrinksCategory>(catalogContent.ContentLink).ToArray();
            
            var vm = new CatalogViewModel()
            {
                Catalog = catalogContent,
                Categories = categories,
                Products = categories.ToDictionary(c => c.ContentLink.ID,
                    category => GetCategoryProducts(catalogContent, category))
            };
            
            return View(vm);
        }

        private CoffeeProduct[] GetCategoryProducts(CatalogContent catalogContent, DrinksCategory category)
        {
            var products =
                _contentLoader.GetChildren<CoffeeProduct>(category.ContentLink, CultureInfo.CurrentCulture);
            return products.ToArray();
        }
    }
}