namespace ClinicBooking.Application.Patients;

public record PatientListItem(
    Guid Id,
    string FullName,
    string Email,
    DateOnly DateOfBirth);

public record SearchPatientsQuery(
    string? Search,
    int Page = 1,
    int PageSize = 20);
    
public record PatientSearchResponse(
    IReadOnlyList<PatientListItem> Items,
    int Total,
    int Page,
    int PageSize);