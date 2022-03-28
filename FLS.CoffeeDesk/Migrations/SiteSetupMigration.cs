using System;
using System.Linq;
using EPiServer.Core;
using EPiServer.Web;
using FLS.CoffeeDesk.Content;
using Microsoft.Extensions.Configuration;

namespace FLS.CoffeeDesk.Migrations
{
    internal class SiteSetupMigration
    {
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;

        private readonly IConfiguration _configuration;

        public SiteSetupMigration(ISiteDefinitionRepository siteDefinitionRepository,
            IConfiguration configuration)
        {
            _siteDefinitionRepository = siteDefinitionRepository;
            _configuration = configuration;
        }

        public SiteDefinition SetupSite()
        {
            var value = _configuration.GetSection("urls").Value;
            var siteUri = new Uri(value.Replace("*", "localhost").Split(";", StringSplitOptions.RemoveEmptyEntries)[0]);

            var currentSites = _siteDefinitionRepository.List();
            var siteDefinition = currentSites.FirstOrDefault(s => s.SiteUrl.ToString() == siteUri.ToString());
            if (siteDefinition != null)
                return siteDefinition;

            var site = new SiteDefinition
            {
                Name = "FLS Coffee Desk",
                SiteUrl = siteUri,
                StartPage = ContentReference.RootPage
            };
            _siteDefinitionRepository.Save(site);
            return site;
        }

        public void UpdateStartPage(ref SiteDefinition site, HomePage homePage)
        {
            var siteEdit = site.CreateWritableClone();
            siteEdit.StartPage = homePage.ContentLink;
            _siteDefinitionRepository.Save(siteEdit);
            site = siteEdit;
        }
    }
}