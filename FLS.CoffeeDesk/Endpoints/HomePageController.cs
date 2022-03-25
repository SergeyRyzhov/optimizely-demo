using EPiServer.Web.Mvc;
using FLS.CoffeeDesk.Content;
using Microsoft.AspNetCore.Mvc;

namespace FLS.CoffeeDesk.Endpoints
{
    public class HomePageController : PageController<HomePage>
    {
        [HttpGet]
        public IActionResult Index(HomePage homePage)
        {
            return View(homePage);
        }
    }
}