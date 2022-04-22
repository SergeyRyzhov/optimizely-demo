using System.ComponentModel;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace FLS.CoffeeDesk.Content
{
    [ContentType(DisplayName = "Custom text",
        Description = "The block that will display custom text",
        GUID = "BC8DCAB1-0704-4DBE-BE61-59FBBBBF5251",
        GroupName = nameof(CoffeeDesk))]
    public class TextBlock : BlockData
    {
        [DisplayName("Rich content")]
        [CultureSpecific]
        public virtual XhtmlString Content { get; set; }
    
        public virtual ContentReference Image { get; set; }
    }
}