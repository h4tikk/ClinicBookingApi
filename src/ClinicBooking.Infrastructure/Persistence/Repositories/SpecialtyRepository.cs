using ClinicBooking.Application.Abstractions;
using ClinicBooking.Application.Specialties;
using ClinicBooking.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence.Repositories;

public class SpecialtyRepository : ISpecialtyRepository
{
    private readonly ClinicDbContext _context;
    public SpecialtyRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public Task<bool> Exists(Guid specialtyId, CancellationToken cancellationToken)
    {
        return _context.Specialties.AnyAsync(s => s.Id == specialtyId, cancellationToken);
    }

    public Task<bool> NameExists(string normalizedName, CancellationToken cancellationToken)
    {
        return _context.Specialties.AnyAsync(s => s.Name == normalizedName, cancellationToken);
    }

    public async Task Add(
        NewSpecialty specialty,
        CancellationToken cancellationToken)
    {
        await _context.Specialties.AddAsync(new Specialty
        {
            Id = specialty.Id,
            Name = specialty.Name
        }, cancellationToken);
    }
}