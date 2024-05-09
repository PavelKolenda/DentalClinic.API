using DentalClinic.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinic.Repository.Configurations;

public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired();

        builder.HasMany(p => p.Dentists)
            .WithOne(s => s.Specialization)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
