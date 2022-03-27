using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;

namespace FLS.CoffeeDesk.Content
{
    [CatalogContentType(DisplayName = "Category",
        Description = "Category in catalog",
        Order = 0,
        GUID = "F62424E8-50EA-44B3-928F-69613A5045E1",
        GroupName = nameof(CoffeeDesk))]
    public class DrinksCategory : NodeContent
    {
    }
}