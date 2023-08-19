using Microsoft.Extensions.DependencyInjection;
using Timelogger.Application.TimeRegistrations.Commands;

namespace Timelogger.Application;

public static class DependencyInjection
{
    public static void ConfigureMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(RegisterTimeCommand).Assembly));
    }
}