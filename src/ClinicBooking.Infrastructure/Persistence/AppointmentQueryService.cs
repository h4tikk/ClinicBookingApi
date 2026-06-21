using ClinicBooking.Application.Appointments;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence;

public class AppointmentQueryService : IAppointmentQueryService
{
    private readonly ClinicDbContext _context;

    public AppointmentQueryService(ClinicDbContext context)
    {
        _context = context;
    }
    public async Task<AppointmentDetailsResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Appointments.AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new AppointmentDetailsResponse(
                a.Id,
                a.DoctorId,
                a.Doctor.FullName,
                a.PatientId,
                a.Patient.FullName,
                a.StartsAt,
                a.EndsAt,
                a.Status.ToString()
            )).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DoctorScheduleItem>> GetDoctorScheduleAsync(Guid doctorId, DateOnly date, CancellationToken cancellationToken)
    {
        var start = new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc), TimeSpan.Zero);
        var end = new DateTimeOffset(date.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc), TimeSpan.Zero);

        return await _context.Appointments.AsNoTracking()
            .Where(a => a.DoctorId == doctorId)
            .Select(a => new DoctorScheduleItem(
                a.Id,
                a.StartsAt,
                a.EndsAt,
                a.Patient.FullName,
                a.Status.ToString()
            )).ToListAsync(cancellationToken);
    }
}