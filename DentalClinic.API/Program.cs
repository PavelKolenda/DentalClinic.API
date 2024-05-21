using DentalClinic.API.Extensions;
using DentalClinic.Repository;
using DentalClinic.Repository.Contracts;
using DentalClinic.Services;
using DentalClinic.Services.Auth;
using DentalClinic.Services.Contracts;
using DentalClinic.Services.Jobs;
using DentalClinic.Services.Options;

using Quartz;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutomaticFluentValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPostgreDB(builder.Configuration);

builder.RegisterAuthentication();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddExceptionHandling();

builder.Services.AddMapper();

builder.Services.AddQuartz(opt =>
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
        .WithCronSchedule(builder.Configuration.GetSection("CreateAppointmentsOptions:CronSchedule").Value);
    });

    //opt.AddTrigger(opt => opt
    //    .ForJob(createAppointmentsForMonth)
    //    .WithIdentity("CreateAppointmentsForMonth-trigger")
    //    .WithSimpleSchedule(schedule => schedule.WithRepeatCount(0)));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.Configure<CreateAppointmentsOptions>
    (builder.Configuration.GetSection("CreateAppointmentsOptions"));

builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();
builder.Services.AddScoped<IPatientsService, PatientsService>();

builder.Services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
builder.Services.AddScoped<IAppointmentsService, AppointmentsService>();

builder.Services.AddScoped<ISpecializationsRepository, SpecializationsRepository>();

builder.Services.AddScoped<IDentistRepository, DentistRepository>();
builder.Services.AddScoped<IDentistsService, DentistsService>();

builder.Services.AddScoped<IWorkingScheduleRepository, WorkingScheduleRepository>();
builder.Services.AddScoped<IWorkingScheduleService, WorkingScheduleService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

app.UseExceptionHandler(options => { });
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
