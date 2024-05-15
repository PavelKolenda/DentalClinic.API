namespace DentalClinic.Shared.DTOs.WorkingSchedules;
public class WorkingScheduleDtoBase
{
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
    public string WorkingDay { get; set; }
}