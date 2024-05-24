﻿using System.Reflection;

using DentalClinic.API.Extensions.ErrorHandling;
using DentalClinic.Repository;
using DentalClinic.Services.Mappings;

using FluentValidation;
using FluentValidation.AspNetCore;

using Mapster;

using Microsoft.EntityFrameworkCore;

namespace DentalClinic.API.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });
    }
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

    public static void AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionLoggingHandler>();
        services.AddExceptionHandler<InvalidRequestExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionsHandling>();
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
