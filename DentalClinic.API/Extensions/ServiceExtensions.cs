using System.Reflection;

using DentalClinic.API.Extensions.ErrorHandling;
using DentalClinic.Repository;
using DentalClinic.Services.Jobs;
using DentalClinic.Services.Mappings;

using FluentValidation;
using FluentValidation.AspNetCore;

using Mapster;

using Microsoft.EntityFrameworkCore;

using Quartz;

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

    public static void AddQuartsAndJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(opt =>
        {
            opt.UseMicrosoftDependencyInjectionJobFactory();

            var createAppointmentsForNextDayKey = new JobKey("CreateAppointmentsForDentistsJob");

            var createAppointmentsForMonth = new JobKey("CreateAppointmentForMonthJob");

            //opt.AddJob<CreateAppointmentsForMonthJob>(opt =>
            //{
            //    opt.WithIdentity(createAppointmentsForMonth);
            //});

            opt.AddJob<CreateDailyAppointmentsJob>(opt =>
            {
                opt.WithIdentity(createAppointmentsForNextDayKey);
            });

            opt.AddTrigger(opt =>
            {
                opt
                .ForJob(createAppointmentsForNextDayKey)
                .WithIdentity("CreateAppointmentsForDentistsJob-trigger")
                .WithCronSchedule(configuration.GetSection("CreateAppointmentsOptions:CronSchedule").Value);
            });

            //opt.AddTrigger(opt => opt
            //    .ForJob(createAppointmentsForMonth)
            //    .WithIdentity("CreateAppointmentsForMonth-trigger")
            //    .WithSimpleSchedule(schedule => schedule.WithRepeatCount(0)));
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
