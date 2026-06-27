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
}