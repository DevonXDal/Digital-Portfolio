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
using Portfolio.Repositories.Interfaces;
using Portfolio.Helpers.Handlers;
using Microsoft.OpenApi.Models;
using System.Reflection;

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

        services.AddSingleton(env); // Early injection of environment variables

        // ----- Set up database, identity, OAuth (IdentityServer) -----
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Configure the _context to use PostgreSQL
            options.UseNpgsql(env.MainDb);

            // Lazy load proxies
            options.UseLazyLoadingProxies();

        });
        services.AddDatabaseDeveloperPageExceptionFilter();

        // ----- Dependency inject database repositories and the handlers
        services.AddScoped<IRepo<ApplicationUser>, RepositoryBase<ApplicationUser>>();
        services.AddScoped<IRepo<BlogLikeUpdate>, RepositoryBase<BlogLikeUpdate>>();
        services.AddScoped<IRepo<RelatedFile>, RepositoryBase<RelatedFile>>();

        services.AddScoped<EmailHandler>();
        services.AddScoped<FileHandler>();


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Digital Portfolio API",
                Description = "An API for a digital portfolio made by Devon Dalrymple"
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
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
        if (!env.IsDevelopment())
        {
            app.UseSpaStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = String.Empty;
            });
        }

        // Reference link: https://stackoverflow.com/questions/61268117/how-to-map-fallback-in-asp-net-core-web-api-so-that-blazor-wasm-app-only-interc
        app.MapWhen(context => !context.Request.Path.StartsWithSegments("/api"), angular => {
            angular.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.Options.DevServerPort = 80;
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        });


        app.UseErrorHandler();
        app.UseSecureHeaders();
        // Use Routing
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        

        app.MapWhen(context => context.Request.Path.StartsWithSegments("/api"), api => {
            api.UseRouting();
            api.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"
                );
                endpoints.MapRazorPages();
                //endpoints.MapFallbackToFile("index.html");
            });
        });
    }
}
