namespace ClinicBooking.Application.Patients;

public interface IPatientQueryService
{
    Task<PatientDetailsResponse?> GetById(
        Guid id,
        CancellationToken cancellationToken);
}