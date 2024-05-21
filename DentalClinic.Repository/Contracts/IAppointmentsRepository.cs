using DentalClinic.Models.Entities;

namespace DentalClinic.Repository.Contracts;
public interface IAppointmentsRepository
{
    Task AssignPatientAsync(Patient patient, Appointment appointment);
    Task<Appointment> CreateAsync(Appointment appointment);
    Task CreateAsync(IEnumerable<Appointment> appointments);
    Task DeleteAsync(int appointmentId, int patientId);
    IQueryable<Appointment> GetAll();
    Task<IEnumerable<Appointment>> GetAvailableAsync(int dentistId);
    Task<Appointment> GetById(int id);
}
