using ClinicBooking.Application.Appointments;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBooking.Api.Controllers;

[ApiController]
[Route("api/appointments")]
public sealed class AppointmentController : ControllerBase
{
    private readonly AppointmentService _service;
    private readonly IAppointmentQueryService _queryService;
    public AppointmentController(AppointmentService service, IAppointmentQueryService queryService)
    {
        _service = service;
        _queryService = queryService;
    }
    [HttpPost]
    [ProducesResponseType<BookAppointmentResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]

    public async Task<ActionResult<BookAppointmentResponse>> Book(
        BookAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Book(request, cancellationToken);
        return result.Status switch
        {
            BookAppointmentStatus.Success => CreatedAtAction(
                nameof(GetById),
                new { id = result.AppointmentId },
                new BookAppointmentResponse(result.AppointmentId!.Value)),

            BookAppointmentStatus.InvalidTimeRange => BadRequest(new ProblemDetails
            {
                Title = "Invalid time range",
                Detail = "StartsAt must be earlier than EndsAt.",
                Status = StatusCodes.Status400BadRequest
            }),

            BookAppointmentStatus.AppointmentInPast => BadRequest(new ProblemDetails
            {
                Title = "Appointment is in the past",
                Detail = "Appointment start time must be in the future.",
                Status = StatusCodes.Status400BadRequest
            }),
            BookAppointmentStatus.DoctorIsBusy => Conflict(new ProblemDetails
            {
                Title = "Doctor is busy",
                Detail = "The doctor already has an appointment in this time range.",
                Status = StatusCodes.Status409Conflict
            }),

            _ => Problem()
        };
    }
    [HttpGet("{id:guid}")]
    [ProducesResponseType<AppointmentDetailsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDetailsResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var appointment = await _queryService.GetByIdAsync(id, cancellationToken);

        if (appointment is null)
        {
            return NotFound();
        }

        return Ok(appointment);
    }

    [HttpGet("doctor/{doctorId:guid}/schedule")]
    public async Task<ActionResult<IReadOnlyCollection<DoctorScheduleItem>>> GetDoctorSchedule(
        Guid doctorId,
        [FromQuery] DateOnly date,
        CancellationToken cancellationToken)
    {
        var schedule = await _queryService.GetDoctorScheduleAsync(doctorId, date, cancellationToken);
        return Ok(schedule);
    }
}