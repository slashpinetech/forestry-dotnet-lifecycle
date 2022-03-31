using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SlashPineTech.Forestry.Lifecycle;

/// <summary>
/// A hosted service that runs startup actions.
/// </summary>
public class LifecycleActionsHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public LifecycleActionsHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // a startup action may contain scoped dependencies, such as a database
        // connection, so we need to create a custom scope here and obtain the
        // startup actions to run directly from the scope.
        using var scope = _serviceProvider.CreateScope();

        var startupActions = scope.ServiceProvider.GetRequiredService<IEnumerable<IStartupAction>>();

        foreach (var startupAction in startupActions)
        {
            await startupAction.OnStartupAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
