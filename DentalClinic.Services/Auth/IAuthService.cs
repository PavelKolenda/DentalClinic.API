using DentalClinic.Shared.DTOs.Patients;

namespace DentalClinic.Services.Auth;
public interface IAuthService
{
    Task<AuthResponse> GetCurrentUser(int userId);
    Task<AuthResponse> Login(PatientLoginDto patientLoginDto);
    Task<AuthResponse> Register(PatientCreateDto patientCreateDto);
}
