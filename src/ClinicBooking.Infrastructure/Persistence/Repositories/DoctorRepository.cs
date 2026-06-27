using ClinicBooking.Application.Abstractions;
using ClinicBooking.Application.Doctors;
using ClinicBooking.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicDbContext _context;
    public DoctorRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public Task<bool> Exists(Guid doctorId, CancellationToken cancellationToken)
    {
        return _context.Doctors.AnyAsync(d => d.Id == doctorId, cancellationToken);
    }

    public async Task Add(NewDoctor doctor, CancellationToken cancellationToken)
    {
        await _context.Doctors.AddAsync(new Doctor
        {
            Id = doctor.Id,
            FullName = doctor.FullName,
            SpecialtyId = doctor.SecialtyId,
            IsActive =  true
            
        }, cancellationToken);
    }

    public async Task<bool> SetActivity(Guid doctorId, bool isActive, CancellationToken cancellationToken)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId, cancellationToken);
        if (doctor is null) return false;
        
        doctor.IsActive = isActive;
        return true;
    }
}
