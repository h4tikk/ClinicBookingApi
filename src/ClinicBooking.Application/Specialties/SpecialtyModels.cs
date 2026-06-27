using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Application.Specialties;

public sealed record CreateSpecialtyRequest(
    [Required] 
    [MaxLength(100)] 
    string Name);

public sealed record CreateSpecialtyResponse(Guid SpecialtyId);

public sealed record SpecialtyResponse(
    Guid Id,
    string Name);

public sealed record NewSpecialty(
    Guid Id,
    string Name);    