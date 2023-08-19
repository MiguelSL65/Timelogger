using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Timelogger.Application.Common;
using Timelogger.Application.Projects.Adapters;
using Timelogger.Application.Projects.Queries.dtos;
using Timelogger.Domain.Repositories;

namespace Timelogger.Application.Projects.Queries;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, GetProjectsQueryResponse>
{
    private readonly IProjectRepository _projectRepository;

    public GetProjectsQueryHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<GetProjectsQueryResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetProjectsOrderedByDeadlineAsync(
            freelancerId: request.FreelancerId,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);

        return new GetProjectsQueryResponse(projects.ToDtoPagedList());
    }
}

public class GetProjectsQuery : IRequest<GetProjectsQueryResponse>
{
    public int FreelancerId { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetProjectsQuery(int freelancerId, int pageNumber, int pageSize)
    {
        FreelancerId = freelancerId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetProjectsQueryResponse
{
    public PagedListDto<GetProjectDto> Projects { get; }

    public GetProjectsQueryResponse(PagedListDto<GetProjectDto> projects)
    {
        Projects = projects;
    }
}