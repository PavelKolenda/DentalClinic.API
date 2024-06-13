namespace DentalClinic.Shared.DTOs.Notifications;
public class NotificationDto
{
    public int Id { get; set; }
    public string Article { get; set; }
    public string Text { get; set; }
    public DateTime SandedAt { get; set; }
}