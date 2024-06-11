namespace DentalClinic.Shared.DTOs.Dentists;

public class DentistCreateDto : DentistDtoBase
{
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly BirthDate { get; set; }
}
