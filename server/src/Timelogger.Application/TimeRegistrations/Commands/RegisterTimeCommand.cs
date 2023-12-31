using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Timelogger.Domain;
using Timelogger.Domain.Crosscutting;
using Timelogger.Domain.Exceptions;
using Timelogger.Domain.Repositories;

namespace Timelogger.Application.TimeRegistrations.Commands;

public class RegisterTimeCommandHandler : IRequestHandler<RegisterTimeCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IClock _clock;
    
    private const int MinTimeFrameInSecondsBetweenRequests = 5;

    public RegisterTimeCommandHandler(
        IProjectRepository projectRepository,
        IClock clock)
    {
        _projectRepository = projectRepository;
        _clock = clock;
    }

    public async Task Handle(RegisterTimeCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProjectByIdAsync(request.ProjectId);

        project.ValidateCanAddTimeRegistration();

        EnsureSufficientTimeSinceLastRequest(project.TimeRegistrationLastInsertedAt);

        var timeRegistration = new TimeRegistration(
            request.ProjectId,
            request.Description,
            request.StartDate,
            request.EndDate);

        timeRegistration.Validate();

        await _projectRepository.CreateTimeRegistrationAsync(timeRegistration);
    }

    private void EnsureSufficientTimeSinceLastRequest(DateTimeOffset? lastCreatedAt)
    {
        if (!lastCreatedAt.HasValue)
        {
            return;
        }

        if ((_clock.UtcNow - lastCreatedAt.Value).TotalSeconds <= MinTimeFrameInSecondsBetweenRequests)
        {
            throw new TimeloggerBusinessException("Cant submit two time registration requests in a row for the same project.");
        }
    }
}

public class RegisterTimeCommand : IRequest
{
    public int ProjectId { get; }
    public string Description { get; }
    public DateTimeOffset StartDate { get; }
    public DateTimeOffset EndDate { get; }

    public RegisterTimeCommand(
        int projectId,
        string description,
        DateTimeOffset startDate,
        DateTimeOffset endDate)
    {
        ProjectId = projectId;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }
}