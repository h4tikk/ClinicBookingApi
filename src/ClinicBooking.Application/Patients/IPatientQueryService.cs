namespace ClinicBooking.Application.Patients;

public interface IPatientQueryService
{
    Task<PatientDetailsResponse?> GetById(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<PatientSearchResponse> Search(
        SearchPatientsQuery query,
        CancellationToken cancellationToken);
}