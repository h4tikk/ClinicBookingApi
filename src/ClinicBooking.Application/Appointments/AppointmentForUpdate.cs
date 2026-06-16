namespace ClinicBooking.Application.Appointments;

public class AppointmentForUpdate
{
    public required Guid Id { get; init; }
    public required string Status { get; set; }
    public string? CancellationReason { get; set; }
}