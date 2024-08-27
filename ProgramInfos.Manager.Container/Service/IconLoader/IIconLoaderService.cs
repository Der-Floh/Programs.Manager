using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Container.Service.IconLoader;

/// <summary>
/// Interface for loading an Icon from an <see cref="IProgramInfoData"/>.
/// </summary>
public interface IIconLoaderService
{
    /// <summary>
    /// Loads an Icon from an <see cref="IProgramInfoData"/>.
    /// </summary>
    /// <param name="programInfoData">The <see cref="IProgramInfoData"/> to load the Icon from.</param>
    /// <returns>A <see cref="MemoryStream"/> containing the image data.</returns>
    MemoryStream? GetIcon(IProgramInfoData programInfoData);
}