using DentalClinic.API.Extensions;
using DentalClinic.Repository;
using DentalClinic.Repository.Contracts;
using DentalClinic.Services;
using DentalClinic.Services.Auth;
using DentalClinic.Services.Contracts;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutomaticFluentValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPostgreDB(builder.Configuration);

builder.RegisterAuthentication();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddMapper();

builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();
builder.Services.AddScoped<IPatientsService, PatientsService>();

builder.Services.AddScoped<ISpecializationsRepository, SpecializationsRepository>();

builder.Services.AddScoped<IDentistRepository, DentistRepository>();
builder.Services.AddScoped<IDentistsService, DentistsService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

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
