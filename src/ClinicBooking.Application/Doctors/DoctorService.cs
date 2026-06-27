using ClinicBooking.Application.Abstractions;

namespace ClinicBooking.Application.Doctors;

public class DoctorService
{
    private readonly IDoctorRepository _doctorRepo;
    private readonly ISpecialtyRepository _specialtyRepo;
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IDoctorRepository doctorRepo, ISpecialtyRepository specialtyRepo, IUnitOfWork unitOfWork)
    {
        _doctorRepo = doctorRepo;
        _specialtyRepo = specialtyRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateDoctorResult> Create(
        CreateDoctorRequest req,
        CancellationToken cancellationToken)
    {
        var specialtyExists = await _specialtyRepo.Exists(req.SpecialtyId, cancellationToken);
        if (!specialtyExists)
            return new CreateDoctorResult(CreateDoctorStatus.SpecialtyNotFound);

        var doctor = new NewDoctor(
            Guid.NewGuid(),
            req.FullName.Trim(),
            req.SpecialtyId);
        
        await _doctorRepo.Add(doctor, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CreateDoctorResult(CreateDoctorStatus.Success, doctor.Id);
    }

    public async Task<ChangeDoctorActivityStatus> ChangeActivity(
        Guid doctorId,
        ChangeDoctorActivityRequest req,
        CancellationToken cancellationToken)
    {
        var updated = await _doctorRepo.SetActivity(doctorId, req.IsActive, cancellationToken);

        if (!updated) return ChangeDoctorActivityStatus.DoctorNotFound;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ChangeDoctorActivityStatus.Success;

    }
}