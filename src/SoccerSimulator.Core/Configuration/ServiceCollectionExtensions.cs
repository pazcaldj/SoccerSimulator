using Microsoft.Extensions.DependencyInjection;
using SoccerSimulator.Core.Services;

namespace SoccerSimulator.Core.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSoccerSimulatorServices(this IServiceCollection services)
    {
        services.AddScoped<IMatchSimulator, MatchSimulator>();
        services.AddScoped<IGroupStageService, GroupStageService>();
        services.AddScoped<ITournamentService, TournamentService>();
                
        return services;
    }
}