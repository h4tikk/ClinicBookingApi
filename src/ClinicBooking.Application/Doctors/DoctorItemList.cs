namespace ClinicBooking.Application.Doctors;

public record DoctorItemList(
    Guid Id,
    string FullName,
    Guid SpecialtyId,
    string SpecialtyName);