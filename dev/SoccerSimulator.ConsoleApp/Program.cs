using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoccerSimulator.Core;
using SoccerSimulator.Core.Configuration;

namespace SoccerSimulator.ConsoleApp;

internal static class Program 
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var simulator = host.Services.GetRequiredService<SoccerSimulatorApp>();
        await simulator.RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSoccerSimulatorServices();
                services.AddScoped<SoccerSimulatorApp>();
                services.AddScoped<IDisplayResults, ConsoleDisplayService>();
            });
}
