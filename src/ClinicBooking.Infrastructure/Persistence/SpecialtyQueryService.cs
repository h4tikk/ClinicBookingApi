using ClinicBooking.Application.Specialties;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence;

public class SpecialtyQueryService : ISpecialtyQueryService
{
    private readonly ClinicDbContext _context;
    public SpecialtyQueryService(ClinicDbContext context)
    {
        _context = context;
    }
    public  Task<SpecialtyResponse?> GetById(Guid specialtyId, CancellationToken cancellationToken)
    {
        return _context.Specialties
            .AsNoTracking()
            .Where(x => x.Id == specialtyId)
            .Select(x => new SpecialtyResponse(x.Id, x.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<SpecialtyResponse>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Specialties
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SpecialtyResponse(x.Id, x.Name))
            .ToListAsync(cancellationToken);    }
}