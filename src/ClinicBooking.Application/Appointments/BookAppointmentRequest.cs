namespace ClinicBooking.Application.Appointments;

public sealed record BookAppointmentRequest(
    Guid PatientId,
    Guid DoctorId,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt
);