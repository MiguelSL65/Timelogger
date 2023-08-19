using System;
using System.Collections.Generic;

namespace Timelogger.Domain.Crosscutting;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int ItemsCount { get; }

    public PagedList(
        IEnumerable<T> items,
        int pageNumber,
        int pageSize,
        int count)
    {
        ItemsCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }
}