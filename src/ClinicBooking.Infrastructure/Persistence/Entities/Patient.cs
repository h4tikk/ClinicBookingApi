namespace ClinicBooking.Infrastructure.Persistence.Entities;

public class Patient
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public DateOnly DateOfBirth { get; set; }
}