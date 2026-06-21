namespace ClinicBooking.Application.Appointments;

public interface IAppointmentQueryService
{
    Task<AppointmentDetailsResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DoctorScheduleItem>> GetDoctorScheduleAsync(Guid doctorId, DateOnly date, CancellationToken cancellationToken);
}