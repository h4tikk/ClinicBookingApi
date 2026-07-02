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

            BookAppointmentStatus.PatientNotFound => BadRequestProblem(
                "Patient not found",
                "The selected patient does not exist."),

            BookAppointmentStatus.DoctorNotFound => BadRequestProblem(
                "Doctor not found",
                "The selected doctor does not exist."),

            BookAppointmentStatus.DoctorIsInactive => ConflictProblem(
                "Doctor is inactive",
                "Appointments cannot be booked for an inactive doctor."),

            BookAppointmentStatus.DoctorIsBusy => ConflictProblem(
                "Doctor is busy",
                "The doctor already has an appointment in this time range."),

            BookAppointmentStatus.InvalidTimeRange => BadRequestProblem(
                "Invalid time range",
                "StartsAt must be earlier than EndsAt."),

            BookAppointmentStatus.AppointmentInPast => BadRequestProblem(
                "Appointment is in the past",
                "The appointment must start in the future."),

            _ => Problem()
        };
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AppointmentDetailsResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var appointment = await _queryService.GetByIdAsync(id, cancellationToken);

        return appointment is null ? NotFound() : Ok(appointment);
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

    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.Confirm(id, cancellationToken);
        return MapStatusResult(result);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancelAppointmentRequest req, CancellationToken cancellationToken)
    {
        var result = await _service.Cancel(id, req, cancellationToken);
        return MapStatusResult(result);
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.Complete(id, cancellationToken);
        if (result == ChangeAppointmentStatus.AppointmentNotFound)
        {
            return ConflictProblem(
                "Appointment has not ended",
                "An appointment can be completed only after its end time");
        }
        return MapStatusResult(result);
    }
    
    private IActionResult MapStatusResult(ChangeAppointmentStatus status)
    {
        return status switch
        {
            ChangeAppointmentStatus.Success => NoContent(),
            ChangeAppointmentStatus.AppointmentNotFound => NotFound(),
            ChangeAppointmentStatus.InvalidState => ConflictProblem(
                "Invalid appointment state",
                "This operation is not allowed for the current appointment state."),
            ChangeAppointmentStatus.TooEarlyToComplete => ConflictProblem(
                "Appointment has not ended",
                "An appointment can be completed only after its end time."),
            _ => Problem()
        };
    }

    private ObjectResult BadRequestProblem(string title, string detail)
    {
        return Problem(
            title: title,
            detail: detail,
            statusCode: StatusCodes.Status400BadRequest);
    }

    private ObjectResult ConflictProblem(string title, string detail)
    {
        return Problem(
            title: title,
            detail: detail,
            statusCode: StatusCodes.Status409Conflict);
    }
}