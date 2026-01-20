using Application.Businesses.Queries.GetDetailsBySlug;
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
    }
}
