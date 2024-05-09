namespace DentalClinic.Models.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Patient> Patients { get; set; }
}