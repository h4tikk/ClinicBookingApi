using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Application.Appointments;

public record BookAppointmentRequest(
    [Required] Guid PatientId,
    [Required] Guid DoctorId,
    [Required] DateTimeOffset StartsAt,
    [Required] DateTimeOffset EndsAt
);