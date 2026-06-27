using ClinicBooking.Application.Doctors;

namespace ClinicBooking.Application.Abstractions;

public interface IDoctorRepository
{
    Task<bool> Exists(Guid doctorId, CancellationToken cancellationToken);
    
    Task Add(NewDoctor doctor, CancellationToken cancellationToken);
    Task<bool> SetActivity(Guid doctorId, bool isActive, CancellationToken cancellationToken);
}