using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PIHLSite.Areas.Identity.Data;
using PIHLSite.Data;

[assembly: HostingStartup(typeof(PIHLSite.Areas.Identity.IdentityHostingStartup))]
namespace PIHLSite.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<PIHLContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("PIHLContextConnection")));

                services.AddDefaultIdentity<PIHLSiteUser>(options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireNonAlphanumeric = false;
                }).AddEntityFrameworkStores<PIHLContext>();
            });
        }
    }
}