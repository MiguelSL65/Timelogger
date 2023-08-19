using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Timelogger.Infrastructure.Persistence.Entities;

namespace Timelogger.Infrastructure.Persistence;

public static class DataSeed
{
    public static void SeedDatabase(this IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetService<ApiContext>();
        
        context.Freelancers.RemoveRange(context.Freelancers);
        context.Projects.RemoveRange(context.Projects);
        context.SaveChanges();
        
        var freelancers = new List<FreelancerEntity>
        {
            new()
            {
                Id = 1,
                Name = "Miguel L.",
                Email = "miguel@example.com",
                CreatedAtUct = DateTimeOffset.UtcNow,
                IsActive = true
            },
            new()
            {
                Id = 2,
                Name = "Jack Green",
                Email = "jack@green.com",
                CreatedAtUct = DateTimeOffset.UtcNow,
                IsActive = true
            }
        };

        var projects = new List<ProjectEntity>
        {
            new()
            {
                Id = 1,
                FreelancerId = 1,
                Name = "e-conomic Interview",
                CompanyName = "E-conomic",
                Deadline = new DateTimeOffset(2023, 12, 31, 12, 00, 00, TimeSpan.Zero),
                IsCompleted = false,
                CreatedAtUct = DateTimeOffset.UtcNow,
                IsActive = true
            },
            new()
            {
                Id = 2,
                FreelancerId = 1,
                Name = "Accounting software",
                CompanyName = "E-conomic",
                Deadline = new DateTimeOffset(2023, 09, 01, 00, 00, 00, TimeSpan.Zero),
                IsCompleted = false,
                CreatedAtUct = DateTimeOffset.UtcNow,
                IsActive = true
            },
            new()
            {
                Id = 3,
                FreelancerId = 1,
                Name = "IDE software",
                CompanyName = "Jetbrains",
                Deadline = new DateTimeOffset(2023, 10, 17, 09, 00, 00, TimeSpan.Zero),
                IsCompleted = false,
                CreatedAtUct = DateTimeOffset.UtcNow,
                IsActive = true
            },
            new()
            {
                Id = 4,
                FreelancerId = 1,
                Name = "Food delivery app",
                CompanyName = "E-conomic",
                Deadline = new DateTimeOffset(2023, 11, 03, 14, 55, 00, TimeSpan.Zero),
                IsCompleted = false,
                CreatedAtUct = DateTimeOffset.UtcNow,
                IsActive = true
            },
            new()
            {
                Id = 5,
                FreelancerId = 2,
                Name = "Learning .NET app",
                CompanyName = "E-conomic",
                Deadline = new DateTimeOffset(2023, 11, 30, 19, 00, 00, TimeSpan.Zero),
                IsCompleted = false,
                CreatedAtUct = DateTimeOffset.UtcNow,
                IsActive = true
            },
            new()
            {
                Id = 6,
                FreelancerId = 2,
                Name = "GTA:6 game",
                CompanyName = "Rockstar",
                Deadline = new DateTimeOffset(2024, 10, 12, 18, 00, 00, TimeSpan.Zero),
                IsCompleted = false,
                CreatedAtUct = DateTimeOffset.UtcNow,
                IsActive = true
            }
        };
        
        context.Freelancers.AddRange(freelancers);
        context.SaveChanges();
        
        context.Projects.AddRange(projects);
        context.SaveChanges();
    }
}