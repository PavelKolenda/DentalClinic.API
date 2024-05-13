using DentalClinic.Shared.DTOs.Patients;

namespace DentalClinic.Services.Auth;
public interface IAuthService
{
    Task<string> Login(PatientLoginDto patientLoginDto);
    Task<string> Register(PatientCreateDto patientCreateDto);
}
