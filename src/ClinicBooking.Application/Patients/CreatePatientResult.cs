namespace ClinicBooking.Application.Patients;

public enum CreatePatientStatus
{
    Success = 1,
    EmailAlreadyExists = 2 
}

public class CreatePatientResult
{
    
    private CreatePatientResult(CreatePatientStatus status, Guid? patientId = null)
    {
        Status = status;
        PatientId = patientId;
    }
    
    public CreatePatientStatus Status { get; }
    public Guid? PatientId { get; }
    
    public static CreatePatientResult Success(Guid patientId)
    {
        return new CreatePatientResult(CreatePatientStatus.Success, patientId);
    }

    public static CreatePatientResult EmailAlreadyExists()
    {
        return new CreatePatientResult(CreatePatientStatus.EmailAlreadyExists);
    }
}