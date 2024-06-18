using DentalClinic.API.Extensions;
using DentalClinic.Repository;
using DentalClinic.Repository.Contracts;
using DentalClinic.Services;
using DentalClinic.Services.Auth;
using DentalClinic.Services.Contracts;
using DentalClinic.Services.Options;

using Quartz;

using QuestPDF.Infrastructure;

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

builder.Services.AddQuartsAndJobs(builder.Configuration);

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
builder.Services.AddScoped<ISpecializationsService, SpecializationsService>();

builder.Services.AddScoped<IDentistRepository, DentistRepository>();
builder.Services.AddScoped<IDentistsService, DentistsService>();

builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();
builder.Services.AddScoped<INotificationsSenderService, AppNotificationsSender>();

builder.Services.AddScoped<IWorkingScheduleRepository, WorkingScheduleRepository>();
builder.Services.AddScoped<IWorkingScheduleService, WorkingScheduleService>();

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IAppointmentInfoDownload, AppointmentInfoDownloadPdf>();

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

app.UseExceptionHandler(options => { });
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.WithOrigins("http://localhost:4200")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
