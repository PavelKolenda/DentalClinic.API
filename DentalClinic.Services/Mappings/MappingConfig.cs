using DentalClinic.Models.Entities;
using DentalClinic.Shared.DTOs.Dentists;
using DentalClinic.Shared.DTOs.Patients;
using DentalClinic.Shared.DTOs.Specializations;
using DentalClinic.Shared.DTOs.WorkingSchedules;

using Mapster;

namespace DentalClinic.Services.Mappings;
public class MappingConfig
{
    public static void Configure()
    {
        #region Patients
        TypeAdapterConfig<PatientCreateDto, Patient>.NewConfig()
            .Map(dest => dest.PasswordHash, src => src.Password);

        TypeAdapterConfig<Patient, PatientDto>.NewConfig();

        TypeAdapterConfig<PatientUpdateDto, Patient>.NewConfig()
            .Map(dest => dest.PasswordHash, src => src.Password);

        TypeAdapterConfig<Patient, PatientCreateDto>.NewConfig();
        #endregion

        #region Dentsits
        TypeAdapterConfig<Dentist, DentistDto>.NewConfig()
            .Map(dest => dest.Specialization, src => src.Specialization.Name);

        TypeAdapterConfig<DentistDtoBase, Dentist>.NewConfig()
            .Ignore(dest => dest.Specialization);
        #endregion

        #region WorkingSchedule
        TypeAdapterConfig<WorkingSchedule, WorkingScheduleDto>.NewConfig();
        TypeAdapterConfig<WorkingScheduleCreateDto, WorkingSchedule>.NewConfig();
        TypeAdapterConfig<WorkingScheduleUpdateDto, WorkingSchedule>.NewConfig();
        #endregion

        #region Specializations
        TypeAdapterConfig<Specialization, SpecializationDto>.NewConfig();
        TypeAdapterConfig<SpecializationCreateDto, Specialization>.NewConfig();
        TypeAdapterConfig<SpecializationUpdateDto, Specialization>.NewConfig();
        #endregion
    }
}
