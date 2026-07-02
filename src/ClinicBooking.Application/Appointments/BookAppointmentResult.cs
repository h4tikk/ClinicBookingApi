namespace ClinicBooking.Application.Appointments;


public class BookAppointmentResult
{
    public BookAppointmentResult(BookAppointmentStatus status,
        Guid? appointmentId = null)
    {
        Status = status;
        AppointmentId = appointmentId; 
    }
    public BookAppointmentStatus Status { get; set; }
    public Guid? AppointmentId { get; set; }

    public static BookAppointmentResult Success(Guid appointmentId)
    {
        return new BookAppointmentResult(
            BookAppointmentStatus.Success,
            appointmentId
        );
    }

    public static BookAppointmentResult InvalidTimeRange()
    {
        return new BookAppointmentResult(BookAppointmentStatus.InvalidTimeRange);
    }    

    public static BookAppointmentResult DoctorIsBusy()
    {
        return new BookAppointmentResult(BookAppointmentStatus.DoctorIsBusy);
    }

    public static BookAppointmentResult AppointmentInPast()
    {
        return new BookAppointmentResult(BookAppointmentStatus.AppointmentInPast);
    }
}