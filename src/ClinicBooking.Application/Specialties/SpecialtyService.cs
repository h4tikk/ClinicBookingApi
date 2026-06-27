using ClinicBooking.Application.Abstractions;

namespace ClinicBooking.Application.Specialties;

public class SpecialtyService
{
    private readonly ISpecialtyRepository _specialtyRepo;
    private readonly IUnitOfWork _unitOfWork;

    public SpecialtyService(ISpecialtyRepository specialtyRepository, IUnitOfWork unitOfWork)
    {
        _specialtyRepo = specialtyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateSpecialtyResult> Create(CreateSpecialtyRequest req,
        CancellationToken cancellationToken)
    {
        var normilizedName = req.Name.Trim();

        var nameExists = await _specialtyRepo.NameExists(
            normilizedName, cancellationToken);

        if (nameExists)
        {
            return CreateSpecialtyResult.NameAlreadyExists();
        }

        var specialty = new NewSpecialty(
            Guid.NewGuid(),
            req.Name);

        await _specialtyRepo.Add(specialty, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return CreateSpecialtyResult.Success(specialty.Id);
    }
}