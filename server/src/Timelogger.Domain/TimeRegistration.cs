using System;
using System.Collections.Generic;
using System.Linq;
using Timelogger.Domain.Exceptions;

namespace Timelogger.Domain;

public class TimeRegistration
{
    public int ProjectId { get; }
    public string Description { get; }
    public DateTimeOffset StartDate { get; }
    public DateTimeOffset EndDate { get; }

    public TimeRegistration(
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

    private const int MinutesPerHour = 60;
    private const double HourRationFor30Minutes = 0.5;
    
    public double GetHoursLogged()
    {
        var totalMinutes = (EndDate - StartDate).TotalMinutes;
        var timeSpentInHours = totalMinutes / MinutesPerHour;

        return timeSpentInHours;
    }

    public void Validate()
    {
        var errors = new List<string>();
        
        ShouldBeMoreThanThirtyMinutes(errors);
        
        if (errors.Any())
        {
            throw new TimeloggerBusinessException(errors);
        }
    }

    private void ShouldBeMoreThanThirtyMinutes(ICollection<string> errors)
    {
        var timeSpentInHours = GetHoursLogged();

        if (timeSpentInHours < HourRationFor30Minutes)
        {
            errors.Add("Time Registration in a project must be at least 30 minutes long!");
        }
    }
}