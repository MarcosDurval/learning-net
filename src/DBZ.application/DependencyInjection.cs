using DBZ.Application.Services;
using DBZ.Application.Abstractions;
using DBZ.Application.Races;
using Microsoft.Extensions.DependencyInjection;

namespace DBZ.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPersonagemService, PersonagemService>();
        services.AddSingleton<IRaceProfileProvider, RaceProfileProvider>();
        services.AddSingleton<IRaceStrategy, RaceStrategy>();

        return services;
    }
}
