using Microsoft.Extensions.Configuration;
using System.Reflection;

public static class ConfigHelper
{
    private static readonly IConfigurationRoot _configuration;

    static ConfigHelper()
    {
        try
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetApplicationRootPath())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            // Validate required settings
            if (string.IsNullOrEmpty(GetBaseUrl()))
                throw new InvalidOperationException("BaseUrl is not configured");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Configuration initialization failed", ex);
        }
    }

    public static IConfiguration GetConfiguration() => _configuration;

    public static string GetSetting(string key) => _configuration[key];

    public static string GetBaseUrl() => _configuration["ApiSettings:BaseUrl"];

    public static string GetUsername() => _configuration["ApiSettings:Username"];

    public static string GetPassword() => _configuration["ApiSettings:Password"];

    private static string GetApplicationRootPath()
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

        if (File.Exists(Path.Combine(assemblyDirectory, "appsettings.json")))
        {
            return assemblyDirectory;
        }

        return Directory.GetCurrentDirectory();
    }
}