namespace ClinicBooking.Application.Specialties;

public enum CreateSpecialtyStatus
{
    Success = 1,
    NameAlreadyExists = 2
}
public class CreateSpecialtyResult
{
    private CreateSpecialtyResult(
        CreateSpecialtyStatus status,
        Guid? specialtyId = null)
    {
        Status = status;
        SpecialtyId = specialtyId;
    }

    public CreateSpecialtyStatus Status { get; }
    public Guid? SpecialtyId { get; }

    public static CreateSpecialtyResult Success(Guid specialtyId)
    {
        return new CreateSpecialtyResult(
            CreateSpecialtyStatus.Success,
            specialtyId);
    }

    public static CreateSpecialtyResult NameAlreadyExists()
    {
        return new CreateSpecialtyResult(
            CreateSpecialtyStatus.NameAlreadyExists);
    }
}