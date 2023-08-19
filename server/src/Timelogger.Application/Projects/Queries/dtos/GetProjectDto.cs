using System;

namespace Timelogger.Application.Projects.Queries.dtos;

public class GetProjectDto
{
    public int Id { get; }
    public int FreelancerId { get; }
    public string Name { get; }
    public string CompanyName { get; }
    public DateTimeOffset Deadline { get; }
    public bool IsCompleted { get; }

    public GetProjectDto(
        int id,
        int freelancerId,
        string name,
        string companyName,
        DateTimeOffset deadline,
        bool isCompleted)
    {
        Id = id;
        FreelancerId = freelancerId;
        Name = name;
        CompanyName = companyName;
        Deadline = deadline;
        IsCompleted = isCompleted;
    }
}