namespace DentalClinic.Shared.DTOs.Dentists;

public class DentistDtoAsUser : DentistDtoBase
{
    public string Email { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly BirthDate { get; set; }
}