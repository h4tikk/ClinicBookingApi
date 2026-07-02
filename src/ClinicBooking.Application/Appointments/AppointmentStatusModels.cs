using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Application.Appointments;

public sealed record CancelAppointmentRequest(
    [Required]
    [MaxLength(500)]
    string Reason);

public enum ChangeAppointmentStatus
{
    Success = 1,
    AppointmentNotFound = 2,
    InvalidState = 3,
    TooEarlyToComplete = 4
}