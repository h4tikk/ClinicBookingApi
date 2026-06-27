using ClinicBooking.Application.Abstractions;

namespace ClinicBooking.Application.Appointments;

public class AppointmentService
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public AppointmentService(
        IAppointmentRepository appointmentRepo,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _appointmentRepo = appointmentRepo;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<BookAppointmentResult> Book(
        BookAppointmentRequest req,
        CancellationToken cancellationToken)
    {
        if(req.StartsAt >= req.EndsAt)
            return BookAppointmentResult.InvalidTimeRange();
        
        var now = _timeProvider.GetUtcNow();

        if(req.StartsAt <= now)
            return BookAppointmentResult.AppointmentInPast();
        
        var hasOverlap = await _appointmentRepo.HasOverlappingAppointment(
            req.DoctorId,
            req.StartsAt,
            req.EndsAt,
            cancellationToken);

        if(hasOverlap)
            return BookAppointmentResult.DoctorIsBusy();

        var appointment = new NewAppointment(
            Id: Guid.NewGuid(),
            PatientId: req.PatientId,
            DoctorId: req.DoctorId,
            StartsAt: req.StartsAt,
            EndsAt: req.EndsAt);

        await _appointmentRepo.Add(appointment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BookAppointmentResult.Success(appointment.Id); 
    }
}