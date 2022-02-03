using Microsoft.Extensions.Configuration;

namespace RetroBot.Launcher.Infrastructure;

internal static class ConfigBuilder
{
    public static IConfiguration Build()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddEnvironmentVariables()
            .Build();
    }
}