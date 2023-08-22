using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Timelogger.Domain.Repositories;

namespace Timelogger.Application.Projects.Commands;

public class CompleteProjectCommandHandler : IRequestHandler<CompleteProjectCommandRequest>
{
    private readonly IProjectRepository _projectRepository;

    public CompleteProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task Handle(CompleteProjectCommandRequest request, CancellationToken cancellationToken)
    {
        await _projectRepository.CompleteProjectAsync(request.ProjectId);
    }
}

public class CompleteProjectCommandRequest : IRequest
{
    public int ProjectId { get; }

    public CompleteProjectCommandRequest(int projectId)
    {
        ProjectId = projectId;
    }
}