using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Application.Patients;

public sealed record CreatePatientRequest(
    [Required]
    [MaxLength(300)]
    string FullName,
    [Required]
    [MaxLength(100)]
    string Email,
    DateOnly DateOfBirth
);