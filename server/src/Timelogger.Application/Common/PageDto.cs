namespace Timelogger.Application.Common;

public class PageDto
{
    public int CurrentPage { get; }
    public int PageSize { get; }
    public int ItemsCount { get; }
    public int TotalPages { get; }

    public PageDto(
        int currentPage,
        int pageSize,
        int itemsCount,
        int totalPages)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        ItemsCount = itemsCount;
        TotalPages = totalPages;
    }
}