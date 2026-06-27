using ClinicBooking.Application.Appointments;

namespace ClinicBooking.Application.Abstractions;

public interface IAppointmentRepository
{
    Task<bool> HasOverlappingAppointment(
        Guid doctorId,
        DateTimeOffset startsAt,
        DateTimeOffset endsAt,
        CancellationToken cancellationToken);

    Task Add(NewAppointment appointment, CancellationToken cancellationToken);

    Task<AppointmentForUpdate?> GetForUpdate(
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