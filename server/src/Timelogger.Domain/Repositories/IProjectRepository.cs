using System;
using System.Threading.Tasks;
using Timelogger.Domain.Crosscutting;
using Timelogger.Domain.dtos;

namespace Timelogger.Domain.Repositories;

public interface IProjectRepository
{
    Task<Project> GetProjectByIdAsync(int projectId);
    Task<DateTimeOffset> GetLastInsertedTimeRegistrationDateAsync(int projectId);
    Task CreateTimeRegistrationAsync(TimeRegistration timeRegistration);
    
    Task<PagedList<GetRegisteredTimeModel>> GetTimeRegistrationsAsync(
        int projectId,
        int pageNumber,
        int pageSize);
    
    Task<PagedList<Project>> GetProjectsOrderedByDeadlineAsync(
        int freelancerId,
        int pageNumber,
        int pageSize);

    Task CompleteProjectAsync(int projectId);
}