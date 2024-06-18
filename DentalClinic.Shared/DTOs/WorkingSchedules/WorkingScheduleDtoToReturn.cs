namespace DentalClinic.Shared.DTOs.WorkingSchedules;

public class WorkingScheduleDtoToReturn
{
    public string DentistName { get; set; }
    public string DentistSurname { get; set; }
    public string? DentistPatronymic { get; set; }
    public IEnumerable<WorkingScheduleDto> WorkingSchedule { get; set; }
}