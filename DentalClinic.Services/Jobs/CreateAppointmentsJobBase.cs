using DentalClinic.Models.Entities;
using DentalClinic.Services.Jobs.MutableTime;

namespace DentalClinic.Services.Jobs;

public abstract class CreateAppointmentsJobBase
{
    protected IEnumerable<Appointment> CreateAppointments(
        int dentistId,
        TimeOnly startTime,
        DateTime dateTime,
        int appointmentsCount,
        int timeToOneAppointmentInMinutes)
    {
        List<Appointment> appointments = [];

        MutableTimeOnly appointmentTime = new(startTime);
        MutableDateTime appointmentDate = new(dateTime);

        for (int i = 0; i < appointmentsCount; i++)
        {
            Appointment appointment = new()
            {
                Date = new DateTime(DateOnly.FromDateTime(appointmentDate.Date), appointmentTime.Time),
                DentistId = dentistId,
            };

            appointments.Add(appointment);

            appointmentTime.AddMinutes(timeToOneAppointmentInMinutes);
        }

        return appointments;
    }
    protected string GetDayOfWeekAsString(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "понедельник",
            DayOfWeek.Tuesday => "вторник",
            DayOfWeek.Wednesday => "среда",
            DayOfWeek.Thursday => "четверг",
            DayOfWeek.Friday => "пятница",
            DayOfWeek.Saturday => "суббота",
            DayOfWeek.Sunday => "воскресенье"
        };
    }
}