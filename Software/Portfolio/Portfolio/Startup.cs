using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using Portfolio.Data;
using Portfolio.Models;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;

namespace Portfolio;

public class Startup
{
    public Startup(IConfiguration configuration)
        => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // ----- Set up static names for environment variables -----
        var env = new Env(Configuration);

        services.AddSingleton<Env>(env); // Early injection of environment variables

        // ----- Set up database, identity, OAuth (IdentityServer) -----
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Configure the _context to use Microsoft SQL Server.
            options.UseMySql(
                env.MainDb,
                new MariaDbServerVersion(new Version(10, 6, 7)),
                mySqlOptions =>
                {
                    mySqlOptions.EnableRetryOnFailure(
                        10, 
                        TimeSpan.FromSeconds(2), 
                        new List<int>{ }
                    );
                }
            );

            // Lazy load proxies <- Avoid this when a collection has known needed
            // navigation properties to avoid N + 1. Eager load instead
            options.UseLazyLoadingProxies();

        });
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<ApplicationUser>();

        services.Configure<IdentityOptions>(options =>
        {
            // Note: to require account confirmation before login,
            // register an email sender service (IEmailSender) and
            // set options.SignIn.RequireConfirmedAccount to true.
            //
            // For more information, visit https://aka.ms/aspaccountconf.
            options.SignIn.RequireConfirmedAccount = false;
        });

        services.AddControllersWithViews().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });
        services.AddRazorPages();

        // ----- Add SPA Static Files -----
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/dist"; // Should never change
        });


        // Admin panel for accessing the database and controlling it through CRUD
        // services.AddCoreAdmin();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Env environmentVars)
    {
        const int maxStaticFileCacheLifeSeconds = 60 * 60 * 24 * 30; // Approx. a month


        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseHsts(); // 30 days by default
        }
        app.UseHttpsRedirection();

        // ----- Add static files -----
        app.UseStaticFiles(new StaticFileOptions
        {
            //https://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/
            OnPrepareResponse = ctx =>
            {

                ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                    "public,max-age=" + maxStaticFileCacheLifeSeconds;
            }
        });
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(environmentVars.FileStoragePath),
            RequestPath = "/filesystem", // Needed for client-side run-at server referencing, "./" is not good here
            OnPrepareResponse = ctx =>
            {

                ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                    "public,max-age=" + maxStaticFileCacheLifeSeconds;
            }
        });


        
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}"
            );
            endpoints.MapRazorPages();
            endpoints.MapFallbackToFile("index.html");
        });

        
        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                spa.UseAngularCliServer(npmScript: "start");
            }
        });
    }
}
