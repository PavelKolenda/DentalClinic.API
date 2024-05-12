using System.Reflection;

using DentalClinic.Repository;
using DentalClinic.Services.Mappings;

using FluentValidation;
using FluentValidation.AspNetCore;

using Mapster;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.API.Extensions;
public static class ServiceExtensions
{
    public static void AddMapper(this IServiceCollection services)
    {
        MappingConfig.Configure();
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
    }

    public static void AddAutomaticFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<Program>();
    }

    public static void AddPostgreDB(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFrameworkNpgsql()
            .AddDbContext<ClinicDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Postgres"));
            });
    }
}
