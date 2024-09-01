using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Reg.Service.IconLoader;

/// <summary>
/// Handles loading of icons for applications from various sources.
/// </summary>
public interface IIconFinderService
{
    /// <summary>
    /// Fetches the icon information for the program info
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> for which the icon path should be fetched.</param>
    /// <returns>An <see cref="IIconInfo"/> containing the icon information.</returns>
    IIconInfo? GetIconInfo(IProgramInfoData programInfoData);
}