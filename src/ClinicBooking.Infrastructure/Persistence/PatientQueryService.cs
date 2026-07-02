using ClinicBooking.Application.Abstractions;
using ClinicBooking.Application.Patients;
using Microsoft.EntityFrameworkCore;


namespace ClinicBooking.Infrastructure.Persistence;

public class PatientQueryService : IPatientQueryService
{
    private readonly ClinicDbContext _context;
    public PatientQueryService(ClinicDbContext context)
    {
        _context = context;
    }
    
    public async Task<PatientDetailsResponse?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Patients.AsNoTracking().Where(x => x.Id == id)
            .Select(x => new PatientDetailsResponse(
                x.Id,
                x.FullName,
                x.Email,
                x.DateOfBirth)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PatientSearchResponse> Search(SearchPatientsQuery query, CancellationToken cancellationToken)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var patients = _context.Patients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search =  query.Search.Trim().ToLower();
            patients = patients.Where(p =>
                p.FullName.ToLower().Contains(search) || p.Email.ToLower().Contains(search));
        }

        var total = await patients.CountAsync(cancellationToken);
        var items = await patients
            .OrderBy(x => x.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new PatientListItem(
                x.Id,
                x.FullName,
                x.Email,
                x.DateOfBirth))
            .ToListAsync(cancellationToken);

        return new PatientSearchResponse(
            items,
            total,
            page,
            pageSize);
    }
}