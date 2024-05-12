namespace DentalClinic.Shared.DTOs.Dentists;

public abstract class DentistDtoBase
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
    public int CabinetNumber { get; set; }
    public string Specialization { get; set; }
}
