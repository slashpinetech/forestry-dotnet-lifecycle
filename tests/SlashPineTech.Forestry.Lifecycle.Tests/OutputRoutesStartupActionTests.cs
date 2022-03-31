using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace SlashPineTech.Forestry.Lifecycle.Tests;

public class OutputRoutesStartupActionTests
{
    [Fact]
    public async Task OnStartupAsync_Logs_All_Routes()
    {
        var actionDescriptors = new ActionDescriptorCollection(new List<ActionDescriptor>
        {
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "no-route-source"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "FOOBAR" }) // non-standard HTTP method
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", null },
                    { "action", null }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = null // will produce "Unknown"
                },
                // don't include any HTTP method constraints here to produce an * for the method
                ActionConstraints = new List<IActionConstraintMetadata>(),
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Unknown" },
                    { "action", "Unknown" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos/{id:int}"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "GET" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Show" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "GET" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Index" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "HEAD" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Head" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "OPTIONS" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Options" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "TRACE" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Trace" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos/new"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "GET" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "New" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "POST" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Create" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos/{id:int}/edit"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "GET" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Edit" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos/{id:int}"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "PUT" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Update" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos/{id:int}"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "PATCH" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "PartialUpdate" }
                }
            },
            new ControllerActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo
                {
                    Template = "foos/{id:int}"
                },
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "DELETE" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", "Foos" },
                    { "action", "Destroy" }
                }
            },
            // let's include a Razor Page for completeness
            new PageActionDescriptor
            {
                AttributeRouteInfo = new AttributeRouteInfo(),
                ActionConstraints = new List<IActionConstraintMetadata>
                {
                    new HttpMethodActionConstraint(new[] { "GET" })
                },
                RouteValues = new Dictionary<string, string?>
                {
                    { "controller", null },
                    { "action", null },
                    { "page", "/SomeFolder/SomePage.cshtml" }
                }
            }
        }, 1);

        var descriptorCollectionProvider = new Mock<IActionDescriptorCollectionProvider>();
        descriptorCollectionProvider
            .Setup(provider => provider.ActionDescriptors)
            .Returns(actionDescriptors);

        var logger = new MockLogger();

        var action = new OutputRoutesStartupAction(descriptorCollectionProvider.Object, logger);
        await action.OnStartupAsync(CancellationToken.None);

        var log = logger.BuildLog();
        log.ShouldBe(@"The following paths were found for the configured controllers:

      * /Unknown (UnknownController#Unknown)
    GET /Unknown (/SomeFolder/SomePage.cshtml)
    GET /foos (FoosController#Index)
   HEAD /foos (FoosController#Head)
OPTIONS /foos (FoosController#Options)
   POST /foos (FoosController#Create)
  TRACE /foos (FoosController#Trace)
    GET /foos/new (FoosController#New)
 DELETE /foos/{id:int} (FoosController#Destroy)
    GET /foos/{id:int} (FoosController#Show)
  PATCH /foos/{id:int} (FoosController#PartialUpdate)
    PUT /foos/{id:int} (FoosController#Update)
    GET /foos/{id:int}/edit (FoosController#Edit)
FOOBAR /no-route-source ()
");
    }

    [Fact]
    public async Task OnStartupAsync_Logs_No_Routes()
    {
        var actionDescriptors = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);

        var descriptorCollectionProvider = new Mock<IActionDescriptorCollectionProvider>();
        descriptorCollectionProvider
            .Setup(provider => provider.ActionDescriptors)
            .Returns(actionDescriptors);

        var logger = new MockLogger();

        var action = new OutputRoutesStartupAction(descriptorCollectionProvider.Object, logger);
        await action.OnStartupAsync(CancellationToken.None);

        var log = logger.BuildLog();
        log.ShouldBe(@"The following paths were found for the configured controllers:

   NONE
");
    }

    private class MockLogger : ILogger<OutputRoutesStartupAction>
    {
        private readonly StringBuilder _log = new();

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _log.Append(state);
        }

        public string BuildLog() => _log.ToString();
    }
}
