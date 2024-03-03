using Programs.Manager.Common.Win.Data;

namespace Programs.Manager.Common.Win.Service;

/// <summary>
/// Service for managing operations related to program information.
/// </summary>
public interface IProgramInfoService
{
    /// <summary>
    /// Modifies an installed program using its modification path.
    /// </summary>
    /// <param name="programInfoData">The program information data containing the modification path.</param>
    /// <param name="additionalArguments">Optional additional arguments for the modification process.</param>
    /// <returns>A task representing the asynchronous operation, returning true if successful.</returns>
    Task<bool> Modify(ProgramInfoData programInfoData, string? additionalArguments = null);

    /// <summary>
    /// Opens the registry editor at the specified registry key of a program.
    /// </summary>
    /// <param name="programInfoData">The program information data containing the registry key.</param>
    /// <returns>A task representing the asynchronous operation, returning true if successful.</returns>
    Task<bool> OpenRegistry(ProgramInfoData programInfoData);

    /// <summary>
    /// Uninstalls a program using its uninstall string.
    /// </summary>
    /// <param name="programInfoData">The program information data containing the uninstall string.</param>
    /// <param name="quiet">Specifies whether to perform a quiet uninstallation.</param>
    /// <returns>A task representing the asynchronous operation, returning true if successful.</returns>
    Task<bool> Uninstall(ProgramInfoData programInfoData, bool quiet = false);

    /// <summary>
    /// Updates the properties of one <see cref="ProgramInfoData"/> object from another.
    /// </summary>
    /// <param name="programInfoData">The object to be updated.</param>
    /// <param name="programInfoDataToCopy">The source object from which to copy properties.</param>
    void UpdateFromDifferent(ProgramInfoData programInfoData, ProgramInfoData programInfoDataToCopy);

    /// <summary>
    /// Fetches fallback properties for a <see cref="ProgramInfoData"/> object.
    /// </summary>
    /// <param name="programInfoData"></param>
    void FetchFallbackProperties(ProgramInfoData programInfoData);
}
