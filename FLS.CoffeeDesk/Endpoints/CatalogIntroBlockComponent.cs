using EPiServer.Web.Mvc;
using FLS.CoffeeDesk.Content;
using Microsoft.AspNetCore.Mvc;

namespace FLS.CoffeeDesk.Endpoints
{
    public class CatalogIntroBlockComponent : BlockComponent<CatalogIntroBlock>
    {
        protected override IViewComponentResult InvokeComponent(CatalogIntroBlock catalogIntro)
        {
            return View(catalogIntro);
        }
    }
}