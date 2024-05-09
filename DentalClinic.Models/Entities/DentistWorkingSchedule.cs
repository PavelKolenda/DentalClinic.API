namespace DentalClinic.Models.Entities;

public class DentistWorkingSchedule
{
    public int DentistId { get; set; }
    public Dentist Dentist { get; set; }
    public int WorkingScheduleId { get; set; }
    public WorkingSchedule WorkingSchedule { get; set; }
}