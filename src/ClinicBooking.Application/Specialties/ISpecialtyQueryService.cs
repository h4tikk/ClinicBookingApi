namespace ClinicBooking.Application.Specialties;

public interface ISpecialtyQueryService
{
    Task<SpecialtyResponse?> GetById(Guid specialtyId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<SpecialtyResponse>> GetAll(CancellationToken cancellationToken);
}