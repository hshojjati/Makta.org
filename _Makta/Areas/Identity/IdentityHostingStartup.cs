using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Makta.Areas.Identity.IdentityHostingStartup))]
namespace Makta.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
        }
    }
}