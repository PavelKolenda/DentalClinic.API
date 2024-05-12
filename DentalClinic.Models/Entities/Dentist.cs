namespace DentalClinic.Models.Entities;

public class Dentist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public int SpecializationId { get; set; }
    public Specialization Specialization { get; set; }
    public int CabinetNumber { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<WorkingSchedule> WorkingSchedule { get; set; }
}