namespace ClinicBooking.Application.Appointments;

public enum AppointmentState
{
    Requested = 1,
    Confirmed = 2,
    Cancelled = 3,
    Completed = 4
}

public record AppointmentSnapshot(Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt,
    AppointmentState Status);