using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timelogger.Domain;
using Timelogger.Domain.Crosscutting;
using Timelogger.Domain.dtos;
using Timelogger.Domain.Exceptions;
using Timelogger.Domain.Repositories;
using Timelogger.Infrastructure.Persistence.Entities;

namespace Timelogger.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApiContext _context;

    public ProjectRepository(ApiContext context)
    {
        _context = context;
    }

    public async Task VerifyIfExistsAsync(int projectId)
    {
        var projectEntity = await _context
            .Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (projectEntity == null)
        {
            throw new EntityNotFoundException(projectId, nameof(Project));
        }
    }

    public async Task<DateTimeOffset> GetLastInsertedTimeRegistrationDateAsync(int projectId)
    {
        return await _context
            .TimeRegistrations
            .AsNoTracking()
            .Where(tr => tr.ProjectId == projectId)
            .Select(tr => tr.CreatedAtUct)
            .LastOrDefaultAsync();
    }

    public async Task CreateTimeRegistrationAsync(TimeRegistration timeRegistration)
    {
        var timeRegistrationEntity = new TimeRegistrationEntity
        {
            ProjectId = timeRegistration.ProjectId,
            Description = timeRegistration.Description,
            StartDate = timeRegistration.StartDate,
            EndDate = timeRegistration.EndDate,
            HoursLogged = timeRegistration.GetHoursLogged(),
            CreatedAtUct = DateTimeOffset.UtcNow,
            IsActive = true
        };

        await _context.TimeRegistrations.AddAsync(timeRegistrationEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedList<GetRegisteredTimeModel>> GetTimeRegistrationsAsync(
        int projectId,
        int pageNumber,
        int pageSize)
    {
        var timeRegistrationsQuery = _context
            .TimeRegistrations
            .Include(tr => tr.Project)
            .AsNoTracking()
            .Where(tr => tr.ProjectId == projectId)
            .Select(tr => new GetRegisteredTimeModel(
                tr.Project.Name,
                tr.Description,
                tr.StartDate,
                tr.EndDate,
                tr.HoursLogged
            ));
        
        var totalCount = await timeRegistrationsQuery.CountAsync();
        
        var timeRegistrations = await timeRegistrationsQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<GetRegisteredTimeModel>(timeRegistrations, pageNumber, pageSize, totalCount);
    }

    public async Task<PagedList<Project>> GetProjectsOrderedByDeadlineAsync(
        int freelancerId,
        int pageNumber,
        int pageSize)
    {
        var projectsQuery = _context
            .Projects
            .AsNoTracking()
            .Where(p => p.FreelancerId == freelancerId)
            .OrderBy(p => p.Deadline)
            .Select(p => new Project(
                p.Id,
                p.FreelancerId,
                p.Name,
                p.CompanyName,
                p.Deadline,
                p.IsCompleted));
        
        var totalCount = await projectsQuery.CountAsync();
        
        var projects = await projectsQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedList<Project>(projects, pageNumber, pageSize, totalCount);
    }
}