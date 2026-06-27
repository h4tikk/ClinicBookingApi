using ClinicBooking.Application.Abstractions;
using ClinicBooking.Application.Appointments;
using ClinicBooking.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ClinicBooking.Infrastructure.Persistence.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicDbContext _context;
    public AppointmentRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task Add(NewAppointment appointment, CancellationToken cancellationToken)
    {
        var entity = new Appointment
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            StartsAt = appointment.StartsAt,
            EndsAt = appointment.EndsAt,
            Status = AppointmentStatus.Requested
        };

        await _context.Appointments.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> Cancel(Guid appointmentId, string reason, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

        if(appointment is null)
            return false;
        if(appointment.Status == AppointmentStatus.Completed)
            return false;
        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancellationReason = reason;
        return true;
    }

    public async Task<bool> Confirm(Guid appointmentId, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

        if(appointment is null)
            return false;
        if(appointment.Status == AppointmentStatus.Cancelled)
            return false;
        appointment.Status = AppointmentStatus.Confrimed;
        return true;
    }

    public async Task<AppointmentForUpdate?> GetForUpdate(Guid appointmentId, CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);
        if(appointment is null)
            return null;
        return new AppointmentForUpdate
        {
            Id = appointment.Id,
            Status = appointment.Status.ToString(),
            CancellationReason = appointment.CancellationReason
        };
    }

    public async Task<bool> HasOverlappingAppointment(Guid doctorId, DateTimeOffset startsAt, DateTimeOffset endsAt, CancellationToken cancellationToken)
    {
        return await _context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.Status != AppointmentStatus.Cancelled &&
            a.StartsAt < endsAt &&
            startsAt < a.EndsAt,
            cancellationToken);
    }
}