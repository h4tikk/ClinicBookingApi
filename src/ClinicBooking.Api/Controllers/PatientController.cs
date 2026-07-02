using ClinicBooking.Application.Patients;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBooking.Api.Controllers;

[ApiController]
[Route("api/patients")]
public sealed class PatientController : ControllerBase
{
    private readonly PatientService _service;
    private readonly IPatientQueryService _queries;

    public PatientController(
        PatientService service,
        IPatientQueryService queries)
    {
        _service = service;
        _queries = queries;
    }

    [HttpPost]
    public async Task<ActionResult<CreatePatientResponse>> Create(
        CreatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Create(request, cancellationToken);

        if (result.Status == CreatePatientStatus.EmailAlreadyExists)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Email already exists",
                Detail = "A patient with this email already exists.",
                Status = StatusCodes.Status409Conflict
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.PatientId },
            new CreatePatientResponse(result.PatientId!.Value));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientDetailsResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var patient = await _queries.GetById(id, cancellationToken);
        return patient is null ? NotFound() : Ok(patient);
    }

    [HttpGet]
    public async Task<ActionResult<PatientSearchResponse>> Search(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _queries.Search(
            new SearchPatientsQuery(search, page, pageSize),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Update(id, request, cancellationToken);

        return result switch
        {
            UpdatePatientStatus.Success => NoContent(),
            UpdatePatientStatus.PatientNotFound => NotFound(),
            UpdatePatientStatus.EmailAlreadyExists => Conflict(new ProblemDetails
            {
                Title = "Email already exists",
                Detail = "Another patient already uses this email.",
                Status = StatusCodes.Status409Conflict
            }),
            _ => Problem()
        };
    }
}