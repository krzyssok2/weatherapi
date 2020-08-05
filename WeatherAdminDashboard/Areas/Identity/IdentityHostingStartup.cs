using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AdminLTE.MVC.Areas.Identity.IdentityHostingStartup))]
namespace AdminLTE.MVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}