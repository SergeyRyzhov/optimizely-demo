using EPiServer.Web.Mvc;
using FLS.CoffeeDesk.Content;
using Microsoft.AspNetCore.Mvc;

namespace FLS.CoffeeDesk.Endpoints
{
    public class CatalogIntroBlockComponent : BlockComponent<CatalogIntroBlock>
    {
        protected override IViewComponentResult InvokeComponent(CatalogIntroBlock catalogIntro)
        {
            // ReSharper disable once Mvc.ViewComponentViewNotResolved
            return View(catalogIntro);
        }
    }
}