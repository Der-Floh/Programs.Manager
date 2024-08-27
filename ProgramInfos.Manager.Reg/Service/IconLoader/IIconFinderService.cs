using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Reg.Data;

namespace ProgramInfos.Manager.Reg.Service.IconLoader;

/// <summary>
/// Handles loading of icons for applications from various sources.
/// </summary>
public interface IIconFinderService
{
    /// <summary>
    /// Retrieves the icon path for the program based on its properties.
    /// </summary>
    /// <param name="programRegInfoData">The <see cref="IProgramInfoData"/> object for which to get the icon path.</param>
    /// <returns>A string representing the icon path or null if no icon file could be found.</returns>
    string? GetIconPath(ProgramInfoData programInfoData);
}