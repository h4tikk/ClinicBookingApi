using ClinicBooking.Application.Abstractions;
using ClinicBooking.Application.Patients;
using ClinicBooking.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ClinicDbContext _context;
    
    public PatientRepository(ClinicDbContext context)
    {
        _context = context;
    }


    public Task<bool> Exists(Guid patientId, CancellationToken cancellationToken)
    {
        return _context.Patients.AnyAsync(p => p.Id == patientId, cancellationToken);
    }
    public Task<bool> EmailExists(string email, CancellationToken cancellationToken)
    {
        return _context.Patients.AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task Add(NewPatient patient, CancellationToken cancellationToken)
    {
        await _context.Patients.AddAsync(new Patient
        {
            Id = patient.Id,
            FullName = patient.FullName,
            Email = patient.Email,
            DateOfBirth = patient.DateOfBirth
        }, cancellationToken);
    }

    public async Task<bool> Update(UpdatedPatient patient, CancellationToken cancellationToken)
    {
        var entity = await _context.Patients.FirstOrDefaultAsync(
            p => p.Id == patient.Id, cancellationToken);

        if (entity is null)
            return false;

        entity.FullName = patient.FullName;
        entity.Email = patient.Email;
        entity.DateOfBirth = patient.DateOfBirth;

        return true;
    }
}