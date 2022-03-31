using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SlashPineTech.Forestry.Lifecycle.Tests;

public class LifecycleActionsHostedServiceTests
{
    [Fact]
    public async Task StartAsync_Executes_Startup_Actions()
    {
        var services = new ServiceCollection();
        services.AddTransient<IStartupAction, ExampleStartupAction>();

        var provider = services.BuildServiceProvider();

        var cancellationTokenSource = new CancellationTokenSource();

        var hostedService = new LifecycleActionsHostedService(provider);
        await hostedService.StartAsync(cancellationTokenSource.Token);
    }

    [Fact]
    public async Task StopAsync_Does_Nothing()
    {
        var services = new ServiceCollection();
        services.AddTransient<IStartupAction, ExampleStartupAction>();

        var provider = services.BuildServiceProvider();

        var cancellationTokenSource = new CancellationTokenSource();

        var hostedService = new LifecycleActionsHostedService(provider);
        await hostedService.StopAsync(cancellationTokenSource.Token);
    }

    private class ExampleStartupAction : IStartupAction
    {
        public Task OnStartupAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
