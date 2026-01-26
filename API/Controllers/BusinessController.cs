using Application.Businesses.Queries.GetBusinesses;
using Application.Businesses.Queries.GetBusinessSetting;
using Application.Businesses.Queries.GetDetailsBySlug;
using Application.Businesses.Queries.GetReviewsForBusiness;
using Application.Businesses.Queries.GetServicesForBusiness;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/businesses")]
    public class BusinessController : ControllerBase
    {
        private readonly ISender _mediator;
        public BusinessController(ISender mediator)
        {
            _mediator = mediator;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetBusinesses([FromQuery] GetBusinessesQuery query, CancellationToken ct)
        {
            var result = await _mediator.Send(query, ct);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Value);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetDetailsBySlug(string slug, CancellationToken ct)
        {
            var query = new GetDetailsBySlugQuery(slug);
            var result = await _mediator.Send(query, ct);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Value);
        }

        [HttpGet("{slug}/services")]
        public async Task<IActionResult> GetServicesForBusiness(string slug, CancellationToken ct)
        {
            var query = new GetServicesForBusinessQuery(slug);
            var result = await _mediator.Send(query, ct);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Value);
        }

        [HttpGet("{slug}/reviews")]
        public async Task<IActionResult> GetReviewsForBusiness(string slug, CancellationToken ct)
        {
            var query = new GetReviewsForBusinessQuery(slug);
            var result = await _mediator.Send(query, ct);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Value);
        }
        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetBusinessSettingsQuery(), ct);
    
            return result.IsSuccess 
                ? Ok(result.Value) 
                : BadRequest(result.ErrorMessage);
        }
    }
}
