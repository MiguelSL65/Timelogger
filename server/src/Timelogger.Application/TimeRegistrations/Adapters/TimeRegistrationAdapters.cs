using System.Linq;
using Timelogger.Application.Common;
using Timelogger.Application.TimeRegistrations.Queries.dtos;
using Timelogger.Domain.Crosscutting;
using Timelogger.Domain.dtos;

namespace Timelogger.Application.TimeRegistrations.Adapters;

public static class TimeRegistrationAdapters
{
    public static PagedListDto<GetTimeRegistrationDto> ToDtoPagedList(this PagedList<GetRegisteredTimeModel> pagedList)
    {
        return new PagedListDto<GetTimeRegistrationDto>(
            items: pagedList.Select(tr => new GetTimeRegistrationDto(
                tr.ProjectName,
                tr.Description,
                tr.StartDate,
                tr.EndDate,
                tr.HoursLogged)),
            pagination: new PageDto(
                pagedList.CurrentPage,
                pagedList.PageSize,
                pagedList.ItemsCount,
                pagedList.TotalPages));
    }
}