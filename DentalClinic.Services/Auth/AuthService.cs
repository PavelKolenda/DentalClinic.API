using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using DentalClinic.Models.Entities;
using DentalClinic.Repository.Contracts;
using DentalClinic.Shared.DTOs.Patients;

using Mapster;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalClinic.Services.Auth;
public class AuthService : IAuthService
{
    private readonly IPatientsRepository _patientsRepository;
    private readonly IdentityService _identityService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IPatientsRepository patientsRepository,
                       IdentityService identityService,
                       ILogger<AuthService> logger)
    {
        _patientsRepository = patientsRepository;
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<string> Register(PatientCreateDto patientCreateDto)
    {
        if (await IsEmailExists(patientCreateDto.Email))
        {
            throw new ArgumentException("Email already used");
        }

        List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Email, patientCreateDto.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                new Claim(ClaimTypes.Role, "Patient")
            ];

        Patient patient = patientCreateDto.Adapt<Patient>();

        Patient createdPatient = await _patientsRepository.CreateAsync(patient);

        ClaimsIdentity claimsIdentity = new(
        [
            new("Id", createdPatient.Id.ToString())
        ]);

        claimsIdentity.AddClaims(claims);

        var token = _identityService.CreateSecurityToken(claimsIdentity);

        var response = _identityService.WriteToken(token);

        return response;
    }

    public async Task<string> Login(PatientLoginDto patientLoginDto)
    {
        Patient? patient = await AuthenticateAsync(patientLoginDto.Email, patientLoginDto.Password);

        if (patient == null)
        {
            throw new ArgumentException("User with provided credentials don't exists");
        }

        var roles = await GetRolesAsync(patient.Id);

        List<Claim> claims =
            [
                new Claim("Id", patient.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, patient.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
            ];

        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = _identityService.CreateSecurityToken(new ClaimsIdentity(claims));

        var response = _identityService.WriteToken(token);

        return response;
    }

    private async Task<Patient?> AuthenticateAsync(string email, string password)
    {
        var patient = await _patientsRepository.GetAll()
            .FirstOrDefaultAsync(p => p.Email == email && p.PasswordHash == password);

        if (patient is null)
        {
            return null;
        }

        return patient;
    }

    private async Task<IEnumerable<string>> GetRolesAsync(int patientId)
    {
        var user = await _patientsRepository
            .GetAll()
            .AsNoTracking()
            .Include(r => r.Roles)
            .FirstOrDefaultAsync(u => u.Id == patientId);

        if (user is null)
        {
            _logger.LogInformation("User with provided Id {patientId}, don't exists", patientId);
            throw new ArgumentException($"User with provided Id {patientId}, don't exists");
        }

        return user.Roles.Select(r => r.Name).ToList();
    }

    private async Task<bool> IsEmailExists(string email)
    {
        return await _patientsRepository
            .GetAll()
            .Select(e => e.Email)
            .AsNoTracking()
            .AnyAsync(p => p == email);
    }
}
