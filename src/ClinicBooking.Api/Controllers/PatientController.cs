using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Application.Patients;

namespace ClinicBooking.Api.Controllers;
[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly IPatientQueryService _queryService;
    private readonly PatientService _service;

    public PatientController(IPatientQueryService queryService, PatientService patientService)
    {
        _queryService = queryService;
        _service = patientService;
    }
    
    [HttpPost]
    [ProducesResponseType<CreatePatientResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
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
        var patient = await _queryService.GetById(id, cancellationToken);

        if (patient is null)
        {
            return NotFound();
        }

        return Ok(patient);
    }
}