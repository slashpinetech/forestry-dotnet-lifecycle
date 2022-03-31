using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace SlashPineTech.Forestry.Lifecycle;

/// <summary>
/// A startup action that outputs all HTTP routes for the application.
/// </summary>
public sealed class OutputRoutesStartupAction : IStartupAction
{
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
    private readonly ILogger<OutputRoutesStartupAction> _logger;

    public OutputRoutesStartupAction(
        IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
        ILogger<OutputRoutesStartupAction> logger)
    {
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        _logger = logger;
    }

    public Task OnStartupAsync(CancellationToken cancellationToken)
    {
        var routes = _actionDescriptorCollectionProvider.ActionDescriptors
            .Items
            .Where(actionDescriptor => actionDescriptor.AttributeRouteInfo != null)
            .Select(RouteLogLine.FromActionDescriptor)
            .ToImmutableSortedSet();

        var msg = new StringBuilder(1024);

        msg.Append("The following paths were found for the configured controllers:");
        msg.AppendLine().AppendLine();

        if (routes.Any())
        {
            foreach (var route in routes)
            {
                msg.Append(route).AppendLine();
            }
        }
        else
        {
            msg.Append("   NONE").AppendLine();
        }

        _logger.LogInformation(msg.ToString());

        return Task.CompletedTask;
    }

    private static string PadMethodName(string methodName)
    {
        return methodName switch
        {
            "GET" => "    GET",
            "HEAD" => "   HEAD",
            "PATCH" => "  PATCH",
            "POST" => "   POST",
            "PUT" => "    PUT",
            "DELETE" => " DELETE",
            "OPTIONS" => "OPTIONS",
            "TRACE" => "  TRACE",
            "*" => "      *",
            _ => methodName
        };
    }

    public record RouteLogLine(
        string HttpMethod,
        string RouteTemplate,
        string RouteSource
    ) : IComparable<RouteLogLine>
    {
        public int CompareTo(RouteLogLine? other)
        {
            var comparison = string.Compare(RouteTemplate, other?.RouteTemplate, StringComparison.Ordinal);
            if (comparison != 0) return comparison;

            comparison = string.Compare(HttpMethod, other?.HttpMethod, StringComparison.Ordinal);
            return comparison;
        }

        public override string ToString()
        {
            return $"{PadMethodName(HttpMethod)} /{RouteTemplate} ({RouteSource})";
        }

        public static RouteLogLine FromActionDescriptor(ActionDescriptor actionDescriptor)
        {
            var httpMethod = actionDescriptor.ActionConstraints?.OfType<HttpMethodActionConstraint>()
                .FirstOrDefault()?.HttpMethods.First() ?? "*";

            var routeTemplate = actionDescriptor.AttributeRouteInfo?.Template ?? "Unknown";
            var controller = actionDescriptor.RouteValues["controller"];
            var action = actionDescriptor.RouteValues["action"];

            // "page" will only exist in the dictionary if services.AddRazorPages() was called.
            string? page = null;
            if (actionDescriptor.RouteValues.ContainsKey("page"))
            {
                page = actionDescriptor.RouteValues["page"];
            }

            string routeSource;
            if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
            {
                routeSource = $"{controller}Controller#{action}";
            }
            else if (!string.IsNullOrEmpty(page))
            {
                routeSource = page;
            }
            else
            {
                routeSource = string.Empty;
            }

            return new RouteLogLine(
                httpMethod,
                routeTemplate,
                routeSource
            );
        }
    }
}
