namespace ClinicBooking.Application.Doctors;

public  record DoctorDetailsResponse(
    Guid Id,
    string FullName,
    Guid SpecialtyId,
    string SpecialtyName,
    bool IsActive);