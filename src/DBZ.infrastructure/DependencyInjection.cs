using DBZ.Application.Abstractions;
using DBZ.Infrastructure.Persistence;
using DBZ.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBZ.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("DbzDb"));

        services.AddScoped<IPersonagemRepository, PersonagemRepository>();

        return services;
    }
}
