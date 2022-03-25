using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace FLS.CoffeeDesk.Content
{
    [CatalogContentType(DisplayName = "Coffee product",
        Description = "Coffee product",
        Order = 1,
        GUID = "6B067656-00E8-4035-84F2-0E6B64CFB572",
        GroupName = nameof(CoffeeDesk))]
    public class CoffeeProduct : ProductContent
    {
        public virtual ContentReference Image { get; set; }
    }
}