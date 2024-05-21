namespace DentalClinic.Shared.DTOs.Appointments;
public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public string DentistName { get; set; }
    public string DentistSurname { get; set; }
    public string DentistPatronymic { get; set; }
    public string DentistSpecialization { get; set; }
    public int DentistCabinetNumber { get; set; }
    public string PatientName { get; set; }
    public string PatientSurname { get; set; }
    public string PatientPatronymic { get; set; }
    public TimeOnly AppointmentTime { get; set; }
    public DateOnly AppointmentDate { get; set; }
}
