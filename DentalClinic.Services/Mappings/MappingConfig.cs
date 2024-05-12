using DentalClinic.Models.Entities;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.DTOs.Patients;

using Mapster;

namespace DentalClinic.Services.Mappings;
public class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<PatientCreateDto, Patient>.NewConfig()
            .Map(dest => dest.PasswordHash, src => src.Password);

        TypeAdapterConfig<Patient, PatientCreateDto>.NewConfig();

        TypeAdapterConfig<Dentist, DentistDto>.NewConfig()
            .Map(dest => dest.Specialization, src => src.Specialization.Name);
        TypeAdapterConfig<DentistDtoBase, Dentist>.NewConfig()
            .Ignore(dest => dest.Specialization);
    }
}
