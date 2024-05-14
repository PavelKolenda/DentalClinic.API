namespace DentalClinic.Shared.DTOs.Patients;

public class PatientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
}