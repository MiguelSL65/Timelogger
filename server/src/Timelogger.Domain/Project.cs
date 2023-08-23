using System;
using Timelogger.Domain.Exceptions;

namespace Timelogger.Domain;

public class Project
{
    public int Id { get; }
    public int FreelancerId { get; }
    public string Name { get; }
    public string CompanyName { get; }
    public DateTimeOffset Deadline { get; }
    public bool IsCompleted { get; }
    public DateTimeOffset? TimeRegistrationLastInsertedAt { get; }

    private Project(
        int id,
        int freelancerId,
        string name,
        string companyName,
        DateTimeOffset deadline,
        bool isCompleted,
        DateTimeOffset? timeRegistrationLastInsertedAt)
    {
        Id = id;
        FreelancerId = freelancerId;
        Name = name;
        CompanyName = companyName;
        Deadline = deadline;
        IsCompleted = isCompleted;
        TimeRegistrationLastInsertedAt = timeRegistrationLastInsertedAt;
    }

    public static Project Create(
        int id,
        int freelancerId,
        string name,
        string companyName,
        DateTimeOffset deadline,
        bool isCompleted)
    {
        return new Project(
            id,
            freelancerId,
            name,
            companyName,
            deadline,
            isCompleted,
            null);
    }
    
    public static Project Create(
        int id,
        int freelancerId,
        string name,
        string companyName,
        DateTimeOffset deadline,
        bool isCompleted,
        DateTimeOffset timeRegistrationLastInsertedAt)
    {
        return new Project(
            id,
            freelancerId,
            name,
            companyName,
            deadline,
            isCompleted,
            timeRegistrationLastInsertedAt);
    }

    public void ValidateCanAddTimeRegistration()
    {
        if (IsCompleted)
        {
            throw new TimeloggerBusinessException("Completed projects can't have new Time Registrations.");
        }
    }
}