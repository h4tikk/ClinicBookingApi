namespace ClinicBooking.Infrastructure.Persistence.Entities;

public class Speciality
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}