using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetroBot.Application.Builder;

namespace RetroBot.Launcher;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
        Thread.Sleep(int.MaxValue);
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var startup = new Startup();
                startup.ConfigureServices(services);
                services.AddHostedService<BotClient>();
            });
}