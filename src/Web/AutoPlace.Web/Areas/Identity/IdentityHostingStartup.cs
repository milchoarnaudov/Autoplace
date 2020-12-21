using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AutoPlace.Web.Areas.Identity.IdentityHostingStartup))]

namespace AutoPlace.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
