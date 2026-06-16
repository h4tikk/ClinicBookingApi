using Microsoft.EntityFrameworkCore;
using ClinicBooking.Infrastructure.Persistence.Entities;

public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) 
        : base(options) { }
    
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Speciality> Specialities => Set<Speciality>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.FullName).HasMaxLength(200);
            builder.Property(p => p.Email).HasMaxLength(50);
            builder.HasIndex(p => p.Email).IsUnique();
        });

        modelBuilder.Entity<Speciality>(builder =>
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).HasMaxLength(100);
            builder.HasIndex(s => s.Name).IsUnique();
        });

        modelBuilder.Entity<Doctor>(builder =>
        {
           builder.HasKey(d => d.Id);
           builder.Property(d => d.FullName).HasMaxLength(200);
           builder.HasOne(d => d.Speciality).WithMany().HasForeignKey(d => d.SpecialityId);
           builder.HasIndex(d => new {d.SpecialityId, d.IsActive});

        });

        modelBuilder.Entity<Appointment>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Patient).WithMany().HasForeignKey(a => a.PatientId);
            builder.HasOne(a => a.Doctor).WithMany().HasForeignKey(a => a.DoctorId);
            builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(50);
            builder.Property(a => a.CancellationReason).HasMaxLength(500);
            builder.HasIndex(a => new {a.DoctorId, a.StartsAt, a.EndsAt});
            builder.Property(a => a.RowVersion).IsRowVersion();
        });
    }


}