namespace ClinicBooking.Infrastructure.Persistence.Entities;

public class Specialty
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}