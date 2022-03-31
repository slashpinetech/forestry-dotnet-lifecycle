using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Xunit;

namespace SlashPineTech.Forestry.Lifecycle.Tests;

public class LifecycleActionExtensionsTests
{
    [Fact]
    public void AddLifecycleActions_Registers_The_HostedService()
    {
        var services = new ServiceCollection();
        services.AddLifecycleActions();

        services.ShouldContain(
            it => it.Lifetime == ServiceLifetime.Singleton &&
                  it.ServiceType == typeof(IHostedService) &&
                  it.ImplementationType == typeof(LifecycleActionsHostedService)
        );
    }

    [Fact]
    public void AddLifecycleActions_Returns_ServiceCollection_For_Chaining()
    {
        var services = new ServiceCollection();
        services.AddLifecycleActions().ShouldNotBeNull();
    }
}
