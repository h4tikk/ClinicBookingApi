using ClinicBooking.Application.Patients;
namespace ClinicBooking.Application.Abstractions;

public interface IPatientRepository
{
    Task<bool> Exists(
        Guid patientId,
        CancellationToken cancellationToken);
    Task<bool> EmailExists(
        string email,
        CancellationToken cancellationToken);

    Task Add(
        NewPatient patient,
        CancellationToken cancellationToken);

    Task<bool> Update(
        UpdatedPatient patient,
        CancellationToken cancellationToken);


}