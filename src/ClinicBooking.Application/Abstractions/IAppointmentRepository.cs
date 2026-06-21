using ClinicBooking.Application.Appointments;

namespace ClinicBooking.Application.Absractions;

public interface IAppointmentRepository
{
    Task<bool> HasOverlappingAppointmentAsync(
        Guid doctorId,
        DateTimeOffset startsAt,
        DateTimeOffset endsAt,
        CancellationToken cancellationToken);

    Task AddAsync(NewAppointment appointment, CancellationToken cancellationToken);

    Task<AppointmentForUpdate?> GetForUpdateAsync(
        Guid appointmentId,
        CancellationToken cancellationToken);

    Task<bool> Confirm(
        Guid appointmentId,
        CancellationToken cancellationToken);

    Task<bool> Cancel(
        Guid appointmentId,
        string reason,
        CancellationToken cancellationToken);
}