using ClinicBooking.Application.Abstractions;

namespace ClinicBooking.Application.Patients;

public class PatientService
{
    private readonly IPatientRepository _patientRepo;
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IPatientRepository patientRepo, IUnitOfWork unitOfWork)
    {
        _patientRepo = patientRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreatePatientResult> Create(CreatePatientRequest req,
        CancellationToken cancellationToken)
    {
        var normilizedEmail = req.Email.Trim().ToLowerInvariant();
        var emailExists = await _patientRepo.EmailExists(
            normilizedEmail, cancellationToken);

        if (emailExists) return CreatePatientResult.EmailAlreadyExists();

        var patient = new NewPatient(
            Guid.NewGuid(),
            req.FullName.Trim(),
            normilizedEmail,
            req.DateOfBirth);
        
        await _patientRepo.Add(patient, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return CreatePatientResult.Success(patient.Id);
    }

    public async Task<UpdatePatientStatus> Update(
        Guid patientId,
        UpdatePatientRequest req,
        CancellationToken cancellationToken)
    {
        var patientExists = await _patientRepo.Exists(
            patientId, cancellationToken);

        if (!patientExists)
            return UpdatePatientStatus.PatientNotFound;
        
        var normilizedEmail = NormalizeEmail(req.Email);
        
        var emailExists = await _patientRepo.EmailExists(normilizedEmail, cancellationToken);
        if (emailExists)
            return UpdatePatientStatus.EmailAlreadyExists;

        var updated = await _patientRepo.Update(new UpdatedPatient(
            patientId,
            req.FullName.Trim(),
            normilizedEmail,
            req.DateOfBirth), cancellationToken);
        if (!updated) return UpdatePatientStatus.PatientNotFound;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return UpdatePatientStatus.Success;
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }
}