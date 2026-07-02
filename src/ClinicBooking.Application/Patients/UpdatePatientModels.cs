using System.ComponentModel.DataAnnotations;

namespace ClinicBooking.Application.Patients;

public record UpdatePatientRequest(
    [Required]
    [MaxLength(200)]
    string FullName,
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    string Email,
    DateOnly DateOfBirth);
    
public record UpdatedPatient(
    Guid Id,
    string FullName,
    string Email,
    DateOnly DateOfBirth);

public enum UpdatePatientStatus
{
    Success = 1,
    PatientNotFound = 2,
    EmailAlreadyExists = 3
}
    
    