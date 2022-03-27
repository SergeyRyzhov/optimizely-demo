using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using FLS.CoffeeDesk.Content;
using FLS.CoffeeDesk.Content.Metadata;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;

namespace FLS.CoffeeDesk.Models
{
    public class CatalogViewModel
    {
        private readonly IPriceService _priceService;

        private readonly BeansOriginSelectionFactory _beansOriginSelectionFactory;

        public CatalogViewModel(IPriceService priceService, BeansOriginSelectionFactory beansOriginSelectionFactory)
        {
            _priceService = priceService;
            _beansOriginSelectionFactory = beansOriginSelectionFactory;
        }

        public CatalogContent Catalog { get; set; }
        
        public DrinksCategory[] Categories { get; set; }
        
        public IDictionary<int, CoffeeProduct[]> Products { get; set; }

        public IDictionary<int, CoffeeVariation[]> Variations { get; set; }

        public string GetDisplayPrice(CoffeeVariation variation)
        {
            return _priceService.GetDefaultPrice(MarketId.Default, DateTime.Now, new CatalogKey(variation.Code), Currency.USD)?.UnitPrice.ToString();
        }

        public object GetDisplayBeansOrigin(CoffeeVariation variation)
        {
            return _beansOriginSelectionFactory.GetSelections(null)
                .FirstOrDefault(o => string.Equals(o.Value.ToString(), variation.BeansOrigin,
                    StringComparison.InvariantCultureIgnoreCase))?.Text;
        }

        public object GetDisplayRoasting(CoffeeVariation variation)
        {
            return ((BeansRoasting)variation.Strength).ToString();
        }
    }
}