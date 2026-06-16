namespace ClinicBooking.Infrastructure.Persistence.Entities;

public class Doctor
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public Guid SpecialityId { get; set; }
    public Speciality Speciality { get; set; } = null;
    public bool  IsActive { get; set; }

}