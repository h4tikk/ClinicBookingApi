namespace ClinicBooking.Application.Doctors;

public record SearchDoctorsQuery(
    Guid? SpecialtyId,
    int Page = 1,
    int PageSize = 20);