using System;

namespace Timelogger.Application.TimeRegistrations.Queries.dtos;

public class GetTimeRegistrationDto
{
    public string ProjectName { get; }
    public string Description { get; }
    public DateTimeOffset StartDate { get; }
    public DateTimeOffset EndDate { get; }
    public double HoursLogged { get; }

    public GetTimeRegistrationDto(
        string projectName,
        string description,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        double hoursLogged)
    {
        ProjectName = projectName;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        HoursLogged = hoursLogged;
    }
}