using DentalClinic.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalClinic.Repository.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Article)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(n => n.SandedAt)
            .IsRequired();
        builder.Property(n => n.Text)
            .HasMaxLength(1250)
            .IsRequired();

        builder.Property(x => x.SandedAt)
            .HasColumnType("timestamp without time zone");
    }
}