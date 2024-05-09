using DentalClinic.Models.Entities;
using DentalClinic.Repository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.Repository;
public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Dentist> Dentists { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<WorkingSchedule> WorkingSchedules { get; set; }
    public DbSet<WorkingDay> WorkingDays { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DentistConfiguration());
        modelBuilder.ApplyConfiguration(new PatientConfiguration());
        modelBuilder.ApplyConfiguration(new SpecializationConfiguration());
    }
}