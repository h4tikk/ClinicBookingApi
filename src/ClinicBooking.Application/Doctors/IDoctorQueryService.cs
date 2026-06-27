namespace ClinicBooking.Application.Doctors;

public interface IDoctorQueryService
{
    Task<DoctorDetailsResponse> GetById(Guid id, CancellationToken cancellationToken);
    Task<PagedResponse<DoctorItemList>> Search(
        SearchDoctorsQuery query, CancellationToken cancellationToken);
}