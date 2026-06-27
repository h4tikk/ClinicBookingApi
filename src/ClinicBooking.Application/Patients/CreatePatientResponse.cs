namespace ClinicBooking.Application.Patients;

public sealed record CreatePatientResponse(Guid PatientId);

public sealed record PatientDetailsResponse(
    Guid Id,
    string FullName,
    string Email,
    DateOnly DateOfBirth);