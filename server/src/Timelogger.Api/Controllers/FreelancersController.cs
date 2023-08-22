using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Api.Requests;
using Timelogger.Api.Responses;
using Timelogger.Application.Freelancers.Queries;

namespace Timelogger.Api.Controllers;

[ApiController]
[Route("api/freelancers/{freelancerId:int}/projects")]
public class FreelancersController : ControllerBase
{
    private readonly IMediator _mediator;

    public FreelancersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(GetProjectsQueryResponse))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.BadRequest, type: typeof(ApplicationError))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ApplicationError))]
    public async Task<IActionResult> GetProjectsAsync(
        [Required] [FromRoute] int freelancerId,
        [Required] [FromQuery] PaginationDto pagination)
    {
        var request = new GetProjectsQuery(
            freelancerId: freelancerId,
            pageNumber: pagination.PageNumber,
            pageSize: pagination.PageSize);

        var response = await _mediator.Send(request);

        return StatusCode((int)HttpStatusCode.OK, response);
    }
}