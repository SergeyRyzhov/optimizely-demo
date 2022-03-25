using System.Collections.Generic;
using EPiServer.Commerce.Catalog.ContentTypes;
using FLS.CoffeeDesk.Content;

namespace FLS.CoffeeDesk.Models
{
    public class CatalogViewModel
    {
        public CatalogContent Catalog { get; set; }
        
        public DrinksCategory[] Categories { get; set; }
        
        public IDictionary<int, CoffeeProduct[]> Products { get; set; }
    }
}