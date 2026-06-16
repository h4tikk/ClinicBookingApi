namespace ClinicBooking.Application.Appointments;

public record NewAppoint(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt
);