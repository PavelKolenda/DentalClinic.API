using DentalClinic.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinic.Repository.Configurations;

public class DentistConfiguration : IEntityTypeConfiguration<Dentist>
{
    public void Configure(EntityTypeBuilder<Dentist> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(d => d.Surname)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(d => d.Patronymic)
            .HasMaxLength(75);

        builder.Property(d => d.CabinetNumber)
            .IsRequired();

        builder.HasMany(a => a.Appointments)
            .WithOne(d => d.Dentist)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

