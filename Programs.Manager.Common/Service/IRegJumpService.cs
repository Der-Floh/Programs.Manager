namespace Programs.Manager.Common.Service.RegJump;

/// <summary>
/// Provides a wrapper for interacting with the RegJump utility to jump to specific registry keys.
/// </summary>
public interface IRegJumpService
{
    /// <summary>
    /// Accepts the EULA for the RegJump utility.
    /// </summary>
    /// <param name="accept">Specifies whether to accept the EULA. The default is true.</param>
    /// <returns>True if the EULA is accepted successfully; otherwise, false.</returns>
    bool AcceptEula(bool accept = true);

    /// <summary>
    /// Opens the registry editor at the specified registry key.
    /// </summary>
    /// <param name="regKey">The registry key to open in the registry editor.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the operation succeeds, otherwise false.</returns>
    Task<bool> OpenAt(string regKey);
}
