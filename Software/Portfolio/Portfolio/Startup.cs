using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using Portfolio.Data;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Portfolio.Models.MainDb;
using Microsoft.AspNetCore.Mvc.Versioning;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Quartz;
using Microsoft.AspNetCore.Authorization;
using Portfolio.Authorization;
using Portfolio.Requirement;
using Portfolio.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
            // Configure the _context to use PostgreSQL
            options.UseNpgsql(env.MainDb);

            // Lazy load proxies
            options.UseLazyLoadingProxies();

        });
        services.AddDatabaseDeveloperPageExceptionFilter();


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .WithHeaders(new string[] {
                        HeaderNames.ContentType,
                        HeaderNames.Authorization,
                    })
                    .WithMethods("GET", "PUT", "POST", "DELETE")
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
            });
        });
        services.AddApiVersioning(o =>
        {
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            o.ReportApiVersions = true;
            o.ApiVersionReader = ApiVersionReader.Combine(
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("X-Version"),
                new MediaTypeApiVersionReader("ver"));

        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var audience = env.Auth0Audience;
                 

            options.Authority =
                  $"https://{env.Auth0Domain}/";
            options.Audience = audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("perform:admin-actions", policy =>
            {
                policy.Requirements.Add(new RbacRequirement("perform:admin-actions"));
            });
        });

        services.AddSingleton<IAuthorizationHandler, RbacHandler>();

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
            app.UseStatusCodePagesWithReExecute("~/error");
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



        app.UseErrorHandler();
        app.UseSecureHeaders();
        app.UseRouting();
        app.UseCors();

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
