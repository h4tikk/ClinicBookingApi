using ClinicBooking.Application.Patients;
namespace ClinicBooking.Application.Abstractions;

public interface IPatientRepository
{
    Task<bool> EmailExists(
        string email,
        CancellationToken cancellationToken);

    Task Add(
        NewPatient patient,
        CancellationToken cancellationToken);
}