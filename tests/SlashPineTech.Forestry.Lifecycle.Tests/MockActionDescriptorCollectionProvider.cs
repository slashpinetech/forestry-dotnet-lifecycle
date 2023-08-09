using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SlashPineTech.Forestry.Lifecycle.Tests;

public class MockActionDescriptorCollectionProvider : IActionDescriptorCollectionProvider
{
    public MockActionDescriptorCollectionProvider() : this(new ActionDescriptorCollection(new List<ActionDescriptor>(), 1))
    {
    }

    public MockActionDescriptorCollectionProvider(ActionDescriptorCollection actionDescriptorCollection)
    {
        ActionDescriptors = actionDescriptorCollection;
    }

    public ActionDescriptorCollection ActionDescriptors { get; }
}
