using DentalClinic.Shared.DTOs.Patients;

namespace DentalClinic.Services.Auth;
public class AuthResponse
{
    public string Token { get; set; }
    public PatientDto User { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
