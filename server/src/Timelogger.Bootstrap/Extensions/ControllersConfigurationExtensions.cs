using Microsoft.Extensions.DependencyInjection;
using Timelogger.Api.Assembly;

namespace Timelogger.Bootstrap.Extensions;

public static class ControllersConfigurationExtensions
{
    public static void AddControllersFromApiAssembly(this IServiceCollection services)
    {
        var apiAssembly = typeof(IApiAssemblyMarker).Assembly;

        services
            .AddControllers()
            .AddApplicationPart(apiAssembly);
    }
}