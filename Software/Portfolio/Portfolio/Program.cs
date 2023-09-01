namespace Portfolio;

public static class Program
{
    public static void Main(string[] args)
    {

        // https://stackoverflow.com/questions/71492149/how-to-get-connectionstring-from-secrets-json-in-asp-net-core-6
        var builder = CreateHostBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;
            config.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (env.IsDevelopment())
            {
                config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            }
            config.AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

        });

        builder.Build().Run();
    }


    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(options => options.UseStartup<Startup>());

}