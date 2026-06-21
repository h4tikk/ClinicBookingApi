using ClinicBooking.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedDevelopmentData(ClinicDbContext db, CancellationToken cancellationToken)
    {
        if (await db.Specialties.AnyAsync(cancellationToken))
        {
            return;
        }

        var therapy = new Specialty
        {
            Id = Guid.NewGuid(),
            Name = "Therapy"
        };

        var cardiology = new Specialty
        {
            Id = Guid.NewGuid(),
            Name = "Cardiology"
        };

        db.Specialties.AddRange(therapy, cardiology);

        db.Doctors.Add(new Doctor
        {
            Id = Guid.NewGuid(),
            FullName = "Dr. Alice Morgan",
            SpecialtyId = therapy.Id
        });

        db.Patients.Add(new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "John Smith",
            Email = "john.smith@example.com",
            DateOfBirth = new DateOnly(1994, 5, 12)
        });

        await db.SaveChangesAsync(cancellationToken);
    }
}