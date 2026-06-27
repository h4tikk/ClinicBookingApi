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
}