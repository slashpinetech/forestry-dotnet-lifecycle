using System.Threading;
using System.Threading.Tasks;

namespace SlashPineTech.Forestry.Lifecycle;

/// <summary>
/// Encapsulates an action that is performed after the application starts
/// up but before the web host begins to accept connections.
/// </summary>
public interface IStartupAction
{
    /// <summary>
    /// Executes the startup action.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has
    /// been aborted.</param>
    Task OnStartupAsync(CancellationToken cancellationToken);
}
