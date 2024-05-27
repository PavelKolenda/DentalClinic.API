using DentalClinic.Shared.DTOs.Patients;

namespace DentalClinic.Services.Auth;
public interface IAuthService
{
    Task<AuthResponse> Login(PatientLoginDto patientLoginDto);
    Task<AuthResponse> Register(PatientCreateDto patientCreateDto);
}
