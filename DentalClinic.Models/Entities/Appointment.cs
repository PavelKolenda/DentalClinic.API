namespace DentalClinic.Models.Entities;

public class Appointment
{
    public int Id { get; set; }
    public int? PatientId { get; set; }
    public Patient? Patient { get; set; }
    public int DentistId { get; set; }
    public Dentist Dentist { get; set; }
    public DateTime Date { get; set; }
}