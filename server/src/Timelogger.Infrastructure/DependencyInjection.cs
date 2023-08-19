using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timelogger.Domain.Crosscutting;
using Timelogger.Domain.Repositories;
using Timelogger.Infrastructure.Persistence;
using Timelogger.Infrastructure.Persistence.Repositories;

namespace Timelogger.Infrastructure;

public static class DependencyInjection
{
    public static void AddTimeloggerDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApiContext>(opt => 
            opt.UseInMemoryDatabase("e-conomic interview"));
    }

    public static void AddRepositoriesServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
    }

    public static void AddSystemClock(this IServiceCollection services)
    {
        services.AddScoped<IClock, Clock>();
    }
}