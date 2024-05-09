namespace DentalClinic.Models.Entities;

public class Specialization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Dentist> Dentists { get; set; }
}