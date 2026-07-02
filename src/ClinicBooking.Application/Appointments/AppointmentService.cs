using ClinicBooking.Application.Abstractions;

namespace ClinicBooking.Application.Appointments;

public class AppointmentService
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IDoctorRepository _doctorRepo;
    private readonly IPatientRepository _patientRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public AppointmentService(
        IAppointmentRepository appointmentRepo,
        IDoctorRepository doctorRepo,
        IPatientRepository patientRepo,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _appointmentRepo = appointmentRepo;
        _doctorRepo = doctorRepo;
        _patientRepo = patientRepo;
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
        
        var patientExists = await _patientRepo.Exists(
            req.PatientId,
            cancellationToken);

        if (!patientExists)
        {
            return new BookAppointmentResult(
                BookAppointmentStatus.PatientNotFound);
        }

        var doctor = await _doctorRepo.GetAvailability(
            req.DoctorId,
            cancellationToken);

        if (doctor is null)
        {
            return new BookAppointmentResult(
                BookAppointmentStatus.DoctorNotFound);
        }

        if (!doctor.IsActive)
        {
            return new BookAppointmentResult(
                BookAppointmentStatus.DoctorIsInactive);
        }

        
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
    
    public Task<ChangeAppointmentStatus> Confirm(
        Guid appointmentId,
        CancellationToken cancellationToken)
    {
        return ChangeStatus(
            appointmentId,
            allowedCurrentState: AppointmentState.Requested,
            targetState: AppointmentState.Confirmed,
            cancellationReason: null,
            cancellationToken);
    }

    public async Task<ChangeAppointmentStatus> Cancel(
        Guid appointmentId,
        CancelAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepo.GetSnapshot(
            appointmentId,
            cancellationToken);

        if (appointment is null)
        {
            return ChangeAppointmentStatus.AppointmentNotFound;
        }

        if (appointment.Status is AppointmentState.Cancelled or AppointmentState.Completed)
        {
            return ChangeAppointmentStatus.InvalidState;
        }

        var changed = await _appointmentRepo.SetStatus(
            appointmentId,
            appointment.Status,
            AppointmentState.Cancelled,
            request.Reason.Trim(),
            cancellationToken);

        if (!changed)
        {
            return ChangeAppointmentStatus.InvalidState;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ChangeAppointmentStatus.Success;
    }

    public async Task<ChangeAppointmentStatus> Complete(
        Guid appointmentId,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepo.GetSnapshot(
            appointmentId,
            cancellationToken);

        if (appointment is null)
        {
            return ChangeAppointmentStatus.AppointmentNotFound;
        }

        if (appointment.Status != AppointmentState.Confirmed)
        {
            return ChangeAppointmentStatus.InvalidState;
        }

        if (_timeProvider.GetUtcNow() < appointment.EndsAt)
        {
            return ChangeAppointmentStatus.TooEarlyToComplete;
        }

        var changed = await _appointmentRepo.SetStatus(
            appointmentId,
            AppointmentState.Confirmed,
            AppointmentState.Completed,
            cancellationReason: null,
            cancellationToken);

        if (!changed)
        {
            return ChangeAppointmentStatus.InvalidState;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ChangeAppointmentStatus.Success;
    }
    
    private async Task<ChangeAppointmentStatus> ChangeStatus(
        Guid appointmentId,
        AppointmentState allowedCurrentState,
        AppointmentState targetState,
        string? cancellationReason,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepo.GetSnapshot(
            appointmentId,
            cancellationToken);

        if (appointment is null)
        {
            return ChangeAppointmentStatus.AppointmentNotFound;
        }

        if (appointment.Status != allowedCurrentState)
        {
            return ChangeAppointmentStatus.InvalidState;
        }

        var changed = await _appointmentRepo.SetStatus(
            appointmentId,
            allowedCurrentState,
            targetState,
            cancellationReason,
            cancellationToken);

        if (!changed)
        {
            return ChangeAppointmentStatus.InvalidState;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ChangeAppointmentStatus.Success;
    }
}