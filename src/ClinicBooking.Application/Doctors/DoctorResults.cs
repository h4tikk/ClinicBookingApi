namespace ClinicBooking.Application.Doctors;

public enum CreateDoctorStatus
{
    Success = 1,
    SpecialtyNotFound = 2
}

public sealed record CreateDoctorResult(
    CreateDoctorStatus Status,
    Guid? DoctorId = null);

public enum ChangeDoctorActivityStatus
{
    Success = 1,
    DoctorNotFound = 2
}