using Microsoft.Extensions.DependencyInjection;

namespace SlashPineTech.Forestry.Lifecycle;

public static class LifecycleActionExtensions
{
    /// <summary>
    /// Adds support for lifecycle actions.
    /// </summary>
    public static IServiceCollection AddLifecycleActions(this IServiceCollection services)
    {
        services.AddHostedService<LifecycleActionsHostedService>();

        return services;
    }
}
