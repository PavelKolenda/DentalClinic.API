using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using DentalClinic.Models.Entities;
using DentalClinic.Models.Exceptions;
using DentalClinic.Repository.Contracts;
using DentalClinic.Shared.DTOs.Patients;

using Mapster;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalClinic.Services.Auth;
public class AuthService : IAuthService
{
    private readonly IPatientsRepository _patientsRepository;
    private readonly IdentityService _identityService;
    private readonly ILogger<AuthService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthService(IPatientsRepository patientsRepository,
                       IdentityService identityService,
                       ILogger<AuthService> logger,
                       IPasswordHasher passwordHasher,
                       IHttpContextAccessor contextAccessor)
    {
        _patientsRepository = patientsRepository;
        _identityService = identityService;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _contextAccessor = contextAccessor;
    }

    public async Task<AuthResponse> Register(PatientCreateDto patientCreateDto)
    {
        if (await IsEmailExists(patientCreateDto.Email))
        {
            throw new InvalidRequestException($"Provided Email: {patientCreateDto.Email} already exists");
        }

        List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Email, patientCreateDto.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                new Claim(ClaimTypes.Role, "Patient")
            ];

        Patient patient = patientCreateDto.Adapt<Patient>();

        patient.PasswordHash = _passwordHasher.Generate(patientCreateDto.Password);

        Patient createdPatient = await _patientsRepository.CreateAsync(patient);

        ClaimsIdentity claimsIdentity = new(
        [
            new("Id", createdPatient.Id.ToString()),
        ]);

        claimsIdentity.AddClaims(claims);

        var token = _identityService.CreateSecurityToken(claimsIdentity);

        var response = _identityService.WriteToken(token);

        var userRoles = await GetRolesAsync(createdPatient.Id);
        AuthResponse authResponse = new()
        {
            Token = response,
            User = createdPatient.Adapt<PatientDto>(),
            Roles = userRoles
        };

        return authResponse;
    }

    public async Task<AuthResponse> Login(PatientLoginDto patientLoginDto)
    {
        Patient? patient = await AuthenticateAsync(patientLoginDto.Email, patientLoginDto.Password);

        if (patient == null)
        {
            throw new InvalidRequestException("User with provided credentials don't exists");
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

        AuthResponse authResponse = new()
        {
            Token = response,
            User = patient.Adapt<PatientDto>(),
            Roles = roles
        };

        return authResponse;
    }

    public async Task<AuthResponse> GetCurrentUser(int userId)
    {
        var patientEntity = await _patientsRepository.GetById(userId, false);
        var patientDto = patientEntity.Adapt<PatientDto>();

        string jwtToken = _contextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var roles = await GetRolesAsync(userId);

        AuthResponse authResponse = new()
        {
            Token = jwtToken,
            User = patientDto,
            Roles = roles
        };

        return authResponse;
    }

    private async Task<Patient?> AuthenticateAsync(string email, string password)
    {
        var patient = await _patientsRepository.GetAll()
            .FirstOrDefaultAsync(p => p.Email == email);

        if (!_passwordHasher.Verify(password, patient.PasswordHash) || patient is null)
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
