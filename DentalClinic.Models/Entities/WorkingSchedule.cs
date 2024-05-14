namespace DentalClinic.Models.Entities;

public class WorkingSchedule
{
    public int Id { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
    public string WorkingDay { get; set; }
    public ICollection<Dentist> Dentists { get; set; }
}