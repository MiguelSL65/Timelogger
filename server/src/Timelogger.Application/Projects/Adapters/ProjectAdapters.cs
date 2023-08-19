using System.Linq;
using Timelogger.Application.Common;
using Timelogger.Application.Projects.Queries.dtos;
using Timelogger.Domain;
using Timelogger.Domain.Crosscutting;

namespace Timelogger.Application.Projects.Adapters;

public static class ProjectsAdapters
{
    public static PagedListDto<GetProjectDto> ToDtoPagedList(this PagedList<Project> pagedList)
    {
        return new PagedListDto<GetProjectDto>(
            items: pagedList.Select(p => new GetProjectDto(
                p.Id,
                p.FreelancerId,
                p.Name,
                p.CompanyName,
                p.Deadline,
                p.IsCompleted)),
            pagination: new PageDto(
                pagedList.CurrentPage,
                pagedList.PageSize,
                pagedList.ItemsCount,
                pagedList.TotalPages));
    }
}