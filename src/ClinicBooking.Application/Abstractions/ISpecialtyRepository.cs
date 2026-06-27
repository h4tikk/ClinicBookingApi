using ClinicBooking.Application.Specialties;

namespace ClinicBooking.Application.Abstractions;

public interface ISpecialtyRepository
{
    Task<bool> Exists(
        Guid specialtyId,
        CancellationToken cancellationToken);

    Task<bool> NameExists(
        string normalizedName,
        CancellationToken cancellationToken);

    Task Add(
        NewSpecialty specialty,
        CancellationToken cancellationToken);
}