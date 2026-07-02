namespace ClinicBooking.Application.Appointments;

public enum BookAppointmentStatus
{
    Success = 1,
    InvalidTimeRange = 2,
    AppointmentInPast = 3,
    PatientNotFound = 4,
    DoctorNotFound = 5,
    DoctorIsInactive = 6,
    DoctorIsBusy = 7
}