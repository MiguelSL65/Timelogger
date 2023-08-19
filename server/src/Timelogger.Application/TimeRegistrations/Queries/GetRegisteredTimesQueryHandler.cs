using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Timelogger.Application.Common;
using Timelogger.Application.TimeRegistrations.Adapters;
using Timelogger.Application.TimeRegistrations.Queries.dtos;
using Timelogger.Domain.Repositories;

namespace Timelogger.Application.TimeRegistrations.Queries;

public class GetRegisteredTimesQueryHandler :
    IRequestHandler<GetRegisteredTimesQuery, GetRegisteredTimesQueryResponse>
{
    private readonly IProjectRepository _projectRepository;

    public GetRegisteredTimesQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<GetRegisteredTimesQueryResponse> Handle(
        GetRegisteredTimesQuery request,
        CancellationToken cancellationToken)
    {
        var timeRegistrations = await _projectRepository.GetTimeRegistrationsAsync(
            projectId: request.ProjectId,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);

        return new GetRegisteredTimesQueryResponse(timeRegistrations.ToDtoPagedList());
    }
}

public class GetRegisteredTimesQuery : IRequest<GetRegisteredTimesQueryResponse>
{
    public int ProjectId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetRegisteredTimesQuery(int projectId, int pageNumber, int pageSize)
    {
        ProjectId = projectId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetRegisteredTimesQueryResponse
{
    public PagedListDto<GetTimeRegistrationDto> TimeRegistrations { get; }

    public GetRegisteredTimesQueryResponse(PagedListDto<GetTimeRegistrationDto> timeRegistrations)
    {
        TimeRegistrations = timeRegistrations;
    }
}