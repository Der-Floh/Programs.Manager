using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Abstractions.Service;

/// <summary>
/// Interface for plugins to implement the service for <see cref="IProgramInfoData"/>
/// </summary>
public interface IProgramInfoDataSourceService
{
    bool IsResponsible(IProgramInfoData programInfoData);

    /// <summary>
    /// Uninstalls the the program.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to uninstall.</param>
    /// <param name="quiet">Optional. Whether to run in quiet mode. (default false)</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation and returns true if the uninstallation is successful.</returns>
    Task<bool> Uninstall(IProgramInfoData programInfoData, bool quiet = false);

    /// <summary>
    /// Modifies the program.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to modify.</param>
    /// <param name="additionalArguments">Optional. Additional arguments to pass to the program.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation and returns true if the modification is successful.</returns>
    Task<bool> Modify(IProgramInfoData programInfoData, string? additionalArguments = null);

    /// <summary>
    /// Opens the location in which the installed program is stored.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to open the install location for.</param>
    void OpenLocation(IProgramInfoData programInfoData);

    /// <summary>
    /// Opens the location in which the program info is stored.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to open the source location for.</param>
    void OpenLocationSource(IProgramInfoData programInfoData);

    /// <summary>
    /// Fetches the icon path for the program info
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> for which the icon path should be fetched.</param>
    /// <returns>A string representing the icon path.</returns>
    string? GetIconPath(IProgramInfoData programInfoData);

    /// <summary>
    /// Fetches fallback values for the program.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to fetch fallback properties for.</param>
    void FetchFallbackProperties(IProgramInfoData programInfoData);

    /// <summary>
    /// Updates an <see cref="IProgramInfoData"/> from a different instance.
    /// </summary>
    /// <param name="programInfoDataToUpdate">The <see cref="IProgramInfoData"/> to update.</param>
    /// <param name="programInfoDataToCopy">The <see cref="IProgramInfoData"/> with which to update.</param>
    void UpdateFromDifferent(IProgramInfoData programInfoDataToUpdate, IProgramInfoData programInfoDataToCopy);
}
