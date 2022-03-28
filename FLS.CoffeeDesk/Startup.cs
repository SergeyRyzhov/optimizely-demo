﻿using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Data;
using EPiServer.Web.Routing;
using FLS.CoffeeDesk.Migrations;
using Mediachase.Commerce.Anonymous;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FLS.CoffeeDesk
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostingEnvironment;

        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
        {
            _webHostingEnvironment = webHostingEnvironment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionStringCommerce = _configuration.GetConnectionString("EcfSqlConnection");

            services.Configure<DataAccessOptions>(o =>
            {
                o.ConnectionStrings.Add(new ConnectionStringOptions
                {
                    ConnectionString = connectionStringCommerce,
                    Name = "EcfSqlConnection"
                });
            });

            services.AddCmsAspNetIdentity<ApplicationUser>(o =>
            {
                if (string.IsNullOrEmpty(o.ConnectionStringOptions?.ConnectionString))
                {
                    o.ConnectionStringOptions = new ConnectionStringOptions()
                    {
                        Name = "EcfSqlConnection",
                        ConnectionString = connectionStringCommerce
                    };
                }
            });
            
            services.AddMvc();
            services.AddCms();
            services.AddCommerce();
            services.AddEmbeddedLocalization<Startup>();
            services.ConfigureApplicationCookie(options => { options.LoginPath = "/util/Login"; });
            services.AddMigrations(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAnonymousId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "Default", pattern: "{controller}/{action}/{id?}");
                endpoints.MapContent();
            });
        }
    }
}