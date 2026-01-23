using Application.Bookings.Commands.CancelBooking;
using Application.Bookings.Commands.CreateBooking;
using Application.Bookings.Queries.GetAvailableSlots;
using Application.Bookings.Queries.GetMyBookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // kräver inlogg för att UserContext ska ha UserId/BusinessId
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/user/bookings")]
        public async Task<IActionResult> GetMyBookings(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetMyBookingsQuery(), ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: /api/bookings
        [HttpPost("create-booking")]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: /api/bookings/{id}/cancel
        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] CancelBookingRequest body, CancellationToken ct)
        {
            var command = new CancelBookingCommand(id, body.CancellationReason);

            var result = await _mediator.Send(command, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: /api/bookings/available-slots?bookingTypeId=...&startDateUtc=...&endDateUtc=...&resourceId=...
        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] Guid bookingTypeId,
            [FromQuery] DateTime startDateUtc,
            [FromQuery] DateTime endDateUtc,
            [FromQuery] Guid? resourceId,
            CancellationToken ct)
        {
            var query = new GetAvailableSlotsQuery(
                bookingTypeId,
                startDateUtc,
                endDateUtc,
                resourceId);

            var result = await _mediator.Send(query, ct);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

    }



    // Liten request-body för cancel endpoint
    public sealed class CancelBookingRequest
    {
        public string? CancellationReason { get; set; }
    }
}
