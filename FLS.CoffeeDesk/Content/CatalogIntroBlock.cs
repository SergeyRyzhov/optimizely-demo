using System.ComponentModel;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace FLS.CoffeeDesk.Content
{
    [ContentType(DisplayName = "Catalog intro",
        Description = "The block that will display some information and link to catalog",
        Order = 0,
        GUID = "FBBA098F-2D31-4E54-A8B7-839D8F9CBA87",
        GroupName = nameof(CoffeeDesk))]
    public class CatalogIntroBlock : BlockData
    {
        [DisplayName("Rich description")]
        [Localizable(true)]
        public virtual XhtmlString Description { get; set; }

        [AllowedTypes(typeof(CatalogContent))]
        public virtual ContentReference Catalog { get; set; }
    }
}