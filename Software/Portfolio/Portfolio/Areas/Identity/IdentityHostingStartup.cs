[assembly: HostingStartup(typeof(Portfolio.Areas.Identity.IdentityHostingStartup))]
namespace Portfolio.Areas.Identity
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
