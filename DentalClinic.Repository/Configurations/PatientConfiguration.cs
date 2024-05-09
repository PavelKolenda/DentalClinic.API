using DentalClinic.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinic.Repository.Configurations;
public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(p => p.Surname)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(p => p.Patronymic)
            .HasMaxLength(75);

        builder.Property(p => p.BirthDate)
            .IsRequired();

        builder.Property(p => p.Email)
            .IsRequired();

        builder.Property(p => p.PasswordHash)
            .IsRequired();

        builder.HasIndex(p => p.Email).IsUnique();

        builder.HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Roles)
            .WithMany(p => p.Patients)
            .UsingEntity<PatientRole>(
                l => l.HasOne<Role>().WithMany().HasForeignKey(r => r.RoleId),
                r => r.HasOne<Patient>().WithMany().HasForeignKey(p => p.PatientId)
            );
    }
}
