using ClinicBooking.Application.Doctors;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence;

public class DoctorQueryService : IDoctorQueryService
{
    private readonly ClinicDbContext _context;
    public DoctorQueryService(ClinicDbContext context)
    {
        _context = context;
    }
    public async Task<PagedResponse<DoctorItemList>> Search(SearchDoctorsQuery query, CancellationToken cancellationToken)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var doctors = _context.Doctors
            .AsNoTracking()
            .Where(d => d.IsActive);

        if (query.SpecialtyId is not null) doctors = doctors.Where(d => d.SpecialtyId == query.SpecialtyId);

        var total = await doctors.CountAsync(cancellationToken);

        var items = await doctors.OrderBy(d => d.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DoctorItemList(
                d.Id,
                d.FullName,
                d.SpecialtyId,
                d.Specialty.Name))
            .ToListAsync(cancellationToken);
        return new PagedResponse<DoctorItemList>(items, total, page, pageSize);
    }

    public Task<DoctorDetailsResponse?> GetById(Guid doctorId, CancellationToken cancellationToken)
    {
        return _context.Doctors
            .AsNoTracking()
            .Where(d => d.Id == doctorId)
            .Select(d => new DoctorDetailsResponse(
                d.Id,
                d.FullName,
                d.SpecialtyId,
                d.Specialty.Name,
                d.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }
}