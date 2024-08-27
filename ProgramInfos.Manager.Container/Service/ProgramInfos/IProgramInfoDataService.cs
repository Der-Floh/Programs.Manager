using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Container.Service.ProgramInfos;

/// <summary>
/// Handles calling plugin implementations for services for an <see cref="IProgramInfoData"/>.
/// </summary>
public interface IProgramInfoDataService
{
    /// <summary>
    /// Fetches the fallback properties for the given <see cref="IProgramInfoData"/>.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to fetch the fallback properties for.</param>
    void FetchFallbackProperties(IProgramInfoData programInfoData);

    /// <summary>
    /// Modifies the installed program for the given <see cref="IProgramInfoData"/>.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to modify.</param>
    /// <param name="additionalArguments">Optional. Additional arguments to modify the installed program with.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation and is true if the modification was successful, false otherwise.</returns>
    Task<bool> Modify(IProgramInfoData programInfoData, string? additionalArguments = null);

    /// <summary>
    /// Uninstalls the installed program for the given <see cref="IProgramInfoData"/>
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to uninstall.</param>
    /// <param name="quiet">Optional. If true, the uninstallation will be quiet.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation and is true if the uninstallation was successful, false otherwise.</returns>
    Task<bool> Uninstall(IProgramInfoData programInfoData, bool quiet = false);

    /// <summary>
    /// Opens the install location of the installed program for the given <see cref="IProgramInfoData"/>
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to open.</param>
    void OpenLocation(IProgramInfoData programInfoData);

    /// <summary>
    /// Opens the source location of where the <see cref="IProgramInfoData"/> came from.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to open.</param>
    void OpenLocationSource(IProgramInfoData programInfoData);

    /// <summary>
    /// Updates an <see cref="IProgramInfoData"/> from a different instance.
    /// </summary>
    /// <param name="programInfoDataToUpdate">The <see cref="IProgramInfoData"/> to update.</param>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> with which to update.</param>
    void UpdateFromDifferent(IProgramInfoData programInfoDataToUpdate, IProgramInfoData programInfoDataToCopy);
}