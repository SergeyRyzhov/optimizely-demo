using EPiServer.Web.Mvc;
using FLS.CoffeeDesk.Content;
using Microsoft.AspNetCore.Mvc;

namespace FLS.CoffeeDesk.Endpoints
{
    public class TextBlockComponent : BlockComponent<TextBlock>
    {
        protected override IViewComponentResult InvokeComponent(TextBlock textBlock)
        {
            return View(textBlock);
        }
    }
}