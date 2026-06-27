using ClinicBooking.Application.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBooking.Api.Controllers;
[ApiController]
[Route("api/doctors")]
public class DoctorController : ControllerBase
{
    private readonly DoctorService _service;
    private readonly IDoctorQueryService _queries;

    public DoctorController(
        DoctorService service,
        IDoctorQueryService queries)
    {
        _service = service;
        _queries = queries;
    }

    [HttpPost]
    public async Task<ActionResult<CreateDoctorResponse>> Create(
        CreateDoctorRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Create(request, cancellationToken);

        if (result.Status == CreateDoctorStatus.SpecialtyNotFound)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Specialty not found",
                Detail = "The selected specialty does not exist.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.DoctorId },
            new CreateDoctorResponse(result.DoctorId!.Value));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorDetailsResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var doctor = await _queries.GetById(id, cancellationToken);
        return doctor is null ? NotFound() : Ok(doctor);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<DoctorItemList>>> Search(
        [FromQuery] Guid? specialtyId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _queries.Search(
            new SearchDoctorsQuery(specialtyId, page, pageSize),
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:guid}/activity")]
    public async Task<IActionResult> ChangeActivity(
        Guid id,
        ChangeDoctorActivityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeActivity(
            id,
            request,
            cancellationToken);

        return result == ChangeDoctorActivityStatus.DoctorNotFound
            ? NotFound()
            : NoContent();
    }
}