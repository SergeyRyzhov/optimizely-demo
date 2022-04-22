using System.ComponentModel;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace FLS.CoffeeDesk.Content
{
    [CatalogContentType(DisplayName = "Coffee product",
        Description = "Coffee product",
        GUID = "6B067656-00E8-4035-84F2-0E6B64CFB572",
        GroupName = nameof(CoffeeDesk))]
    public class CoffeeProduct : ProductContent
    {
        [DisplayName("Rich description")]
        [CultureSpecific]
        public virtual XhtmlString Description { get; set; }
        public virtual ContentReference Image { get; set; }
    }
}