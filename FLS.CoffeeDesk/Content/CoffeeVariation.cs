using System.ComponentModel.DataAnnotations;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using FLS.CoffeeDesk.Content.Metadata;

namespace FLS.CoffeeDesk.Content
{
    [CatalogContentType(DisplayName = "Coffee variation",
        Description = "Coffee variation",
        GUID = "3398371F-88F3-40EE-B0F1-D38F334F62D8",
        GroupName = nameof(CoffeeDesk))]
    public class CoffeeVariation : VariationContent
    {
        [Display(Name = "Beans Origin Country")]
        [SelectOne(SelectionFactoryType = typeof(BeansOriginSelectionFactory))]
        public virtual string BeansOrigin { get; set; }
    
        [SelectOne(SelectionFactoryType = typeof(BeansRoastingSelectionFactory))]
        public virtual int Strength { get; set; }
    
        public virtual ContentReference Image { get; set; }
    }
}