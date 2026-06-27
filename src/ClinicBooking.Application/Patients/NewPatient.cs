namespace ClinicBooking.Application.Patients;

public record NewPatient(
    Guid Id,
    string FullName,
    string Email,
    DateOnly DateOfBirth);