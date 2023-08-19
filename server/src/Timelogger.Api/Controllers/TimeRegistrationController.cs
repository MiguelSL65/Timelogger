using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Api.Requests;
using Timelogger.Api.Responses;
using Timelogger.Application.TimeRegistrations.Commands;
using Timelogger.Application.TimeRegistrations.Queries;

namespace Timelogger.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId:int}/time-registrations")]
public class TimeRegistrationController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimeRegistrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.Created)]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.BadRequest, type: typeof(ApplicationError))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ApplicationError))]
    public async Task<IActionResult> CreateTimeRegistrationAsync(
        [Required][FromRoute] int projectId,
        [Required][FromBody] CreateTimeRegistrationDto requestBody)
    {
        var command = new RegisterTimeCommand(
            projectId: projectId,
            description: requestBody.Description,
            startDate: requestBody.StartDate,
            endDate: requestBody.EndDate);

        await _mediator.Send(command);
        
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpGet]
    [Consumes(contentType: "application/json")]
    [Produces(contentType: "application/json")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(GetRegisteredTimesQueryResponse))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ApplicationError))]
    public async Task<IActionResult> GetTimeRegistrationsAsync(
        [Required][FromRoute] int projectId,
        [Required] [FromQuery] PaginationDto pagination)
    {
        var request = new GetRegisteredTimesQuery(
            projectId: projectId,
            pageNumber: pagination.PageNumber,
            pageSize: pagination.PageSize);
        
        var response = await _mediator.Send(request);
        
        return StatusCode(statusCode: (int)HttpStatusCode.OK, value: response);
    }
}