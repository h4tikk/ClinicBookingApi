using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Application.Doctors;

public record CreateDoctorRequest(
    [Required] [MaxLength(200)] string FullName,
    Guid SpecialtyId);

public sealed record CreateDoctorResponse(Guid DoctorId);

public sealed record ChangeDoctorActivityRequest(bool IsActive);

public record NewDoctor(
    Guid Id,
    string FullName,
    Guid SecialtyId);