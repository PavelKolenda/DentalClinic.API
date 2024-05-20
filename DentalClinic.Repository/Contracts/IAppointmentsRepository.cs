using DentalClinic.Models.Entities;

namespace DentalClinic.Repository.Contracts;
public interface IAppointmentsRepository
{
    Task<Appointment> CreateAppointment(Appointment appointment);
    Task CreateAppointments(IEnumerable<Appointment> appointments);
    Task DeleteAppointment(int appointmentId, int patientId);
    IQueryable<Appointment> GetAll();
}
