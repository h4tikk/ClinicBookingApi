using ClinicBooking.Application.Specialties;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBooking.Api.Controllers;

[ApiController]
[Route("api/specialties")]
public sealed class SpecialtiesController : ControllerBase
{
    private readonly ISpecialtyQueryService _queryService;
    private readonly SpecialtyService _service;

    public SpecialtiesController(ISpecialtyQueryService queryService, SpecialtyService service)
    {
        _queryService = queryService;
        _service = service;
    }
    [HttpPost]
    [ProducesResponseType<CreateSpecialtyResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CreateSpecialtyResponse>> Create(
        CreateSpecialtyRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Create(request, cancellationToken);

        if (result.Status == CreateSpecialtyStatus.NameAlreadyExists)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Specialty already exists",
                Detail = "A specialty with this name already exists.",
                Status = StatusCodes.Status409Conflict
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.SpecialtyId },
            new CreateSpecialtyResponse(result.SpecialtyId!.Value));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<SpecialtyResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var specialties = await _queryService.GetAll(cancellationToken);
        return Ok(specialties);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SpecialtyResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var specialty = await _queryService.GetById(id, cancellationToken);
        return specialty is null ? NotFound() : Ok(specialty);
    }
}