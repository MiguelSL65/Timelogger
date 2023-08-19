using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Api.Requests;
using Timelogger.Api.Responses;
using Timelogger.Application.Projects.Queries;

namespace Timelogger.Api.Controllers;

[ApiController]
[Route("api/freelancers/{freelancerId:int}/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(GetProjectsQueryResponse))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.BadRequest, type: typeof(ApplicationError))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ApplicationError))]
    public async Task<IActionResult> GetTimeRegistrationsAsync(
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