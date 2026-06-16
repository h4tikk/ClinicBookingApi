namespace ClinicBooking.Application.Appointments;

public enum BookAppointmentStatus
{
    Success = 1,
    InvalidTimeRange = 2,
    DoctorIsBusy = 3,
    AppointmentInPast = 4
}