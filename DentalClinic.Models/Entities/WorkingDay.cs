namespace DentalClinic.Models.Entities;

public class WorkingDay
{
    public int Id { get; set; }
    public string Day { get; set; }
    public ICollection<WorkingSchedule> WorkingSchedule { get; set; }
}