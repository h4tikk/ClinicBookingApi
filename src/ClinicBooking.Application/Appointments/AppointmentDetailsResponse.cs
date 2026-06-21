namespace ClinicBooking.Application.Appointments;

public sealed record AppointmentDetailsResponse(
    Guid Id,
    Guid DoctorId,
    string DoctorName,
    Guid PatientId,
    string PatientName,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt,
    string Status
);

public sealed record DoctorScheduleItem(
    Guid AppointmentId,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt,
    string PatientName,
    string Status
);