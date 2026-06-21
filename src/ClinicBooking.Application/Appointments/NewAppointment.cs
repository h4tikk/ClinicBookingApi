namespace ClinicBooking.Application.Appointments;

public record NewAppointment(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt
);