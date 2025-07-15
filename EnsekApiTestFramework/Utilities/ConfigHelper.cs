using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Collections.Generic;

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
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Configuration initialization failed", ex);
        }
    }

    public static string GetSetting(string key) => _configuration[key];

    public static string BaseUrl => _configuration["ApiSettings:BaseUrl"];
    public static string Username => _configuration["ApiSettings:Username"];
    public static string Password => _configuration["ApiSettings:Password"];
    
    public static Dictionary<string, int> GetEnergyMappings()
    {
        var section = _configuration.GetSection("EnergyMappings");
        var dict = new Dictionary<string, int>();
        
        foreach (var child in section.GetChildren())
        {
            if (int.TryParse(child.Value, out int value))
            {
                dict[child.Key] = value;
            }
        }
        return dict;
    }

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