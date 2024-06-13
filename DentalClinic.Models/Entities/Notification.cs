namespace DentalClinic.Models.Entities;
public class Notification
{
    public int Id { get; set; }
    public string Article { get; set; }
    public string Text { get; set; }
    public DateTime SandedAt { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
}
