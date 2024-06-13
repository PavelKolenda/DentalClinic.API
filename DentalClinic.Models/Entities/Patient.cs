namespace DentalClinic.Models.Entities;
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Role> Roles { get; set; }
    public ICollection<Notification> Notifications { get; set; }
}