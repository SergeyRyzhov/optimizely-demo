using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace FLS.CoffeeDesk.Content
{
    [ContentType(DisplayName = "Home page",
        Description = "The page that will be displayed to the user when he enters the site",
        Order = 0,
        GUID = "52C7B793-D3AA-47DC-8E02-71AD53899E40",
        GroupName = nameof(CoffeeDesk))]
    public class HomePage : PageData
    {
        [Editable(true)]
        [Localizable(true)]
        public virtual ContentArea Content { get; set; }
    }
}