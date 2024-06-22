using DentalClinic.Services.Contracts;

using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace DentalClinic.Services;
public class AppointmentInfoDownloadPdf : IAppointmentInfoDownload
{
    private readonly IAppointmentsService _appointmentsService;

    public AppointmentInfoDownloadPdf(IAppointmentsService appointmentsService)
    {
        _appointmentsService = appointmentsService;
    }

    public async Task<byte[]> Download(int appointmentId)
    {
        var appointment = await _appointmentsService.GetById(appointmentId);

        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(3);
                page.Size(PageSizes.A6);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(16));

                page.Header()
                    .AlignCenter()
                    .Text("Талон")
                    .SemiBold().FontSize(24).FontColor(Colors.Grey.Darken4);

                page.Content()
                    .Column(column =>
                    {
                        column.Item()
                            .Text(text =>
                            {
                                text.Span("1. Врач ");
                                text.Span($"{appointment.DentistSurname} {appointment.DentistName} {appointment?.DentistPatronymic}").Underline();
                            });

                        column.Item().Text(text =>
                        {
                            text.Span($"2. Специальность ");
                            text.Span($"{appointment.DentistSpecialization}").Underline();
                        });


                        column.Item().Text(text =>
                        {
                            text.Span("3. Кабинет ");
                            text.Span($"{appointment.DentistCabinetNumber}").Underline();
                        });


                        column.Item().Text(text =>
                        {
                            text.Span("4. Дата приема ");
                            text.Span($"{appointment.AppointmentDate.Day}.${appointment.AppointmentDate.Month}.{appointment.AppointmentDate.Year}")
                            .Underline();
                        });


                        column.Item().Text(text =>
                        {
                            text.Span("5. Время приема ");
                            text.Span($"{appointment.AppointmentTime}").Underline();
                        });


                        column.Item().Text(text =>
                        {
                            text.Span("6. Пациент ");
                            text.Span($"{appointment.PatientName} {appointment.PatientSurname} {appointment?.PatientPatronymic}").Underline();
                        });
                    });
            });
        }).GeneratePdf();

        return pdfBytes;
    }
}