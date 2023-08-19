using System.Collections.Generic;

namespace Timelogger.Application.Common;

public class PagedListDto<T>
{
    public IEnumerable<T> Items { get; }
    public PageDto Pagination { get; }

    public PagedListDto(
        IEnumerable<T> items,
        PageDto pagination)
    {
        Items = items;
        Pagination = pagination;
    }
}