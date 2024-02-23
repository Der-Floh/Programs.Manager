using Programs.Manager.Reader.Win.Data;
using System.Drawing;

namespace Programs.Manager.Reader.Win.Service.IconLoader;

/// <summary>
/// Handles loading of icons for applications from various sources.
/// </summary>
public interface IIconLoaderService
{
    /// <summary>
    /// Retrieves the icon for the program based on its properties.
    /// </summary>
    /// <param name="programRegInfoData">The <see cref="ProgramRegInfoData"/> object for which to load the icon.</param>
    /// <returns>A <see cref="Bitmap"/> object representing the icon, or null if no icon could be found.</returns>
    Bitmap? GetIcon(ProgramRegInfoData programRegInfoData);

    /// <summary>
    /// Retrieves an icon from a file.
    /// </summary>
    /// <param name="iconPath">The path to the icon file.</param>
    /// <param name="iconIndex">The <see cref="IconIndex"/> object in which the indicies of the icon to use are located.</param>
    /// <returns>A <see cref="Bitmap"/> object representing the icon, or null if the icon couldn't be loaded.</returns>
    Bitmap? GetIconFromFile(string iconPath, IconIndex? iconIndex = null);

    /// <summary>
    /// Removes the icon index from a filepath, if present.
    /// </summary>
    /// <param name="filePath">The path from which to remove the index.</param>
    /// <returns>The cleaned filepath.</returns>
    string RemoveIconIndex(string filePath);

    /// <summary>
    /// Removes the icon index from a filepath if present and returns the cleaned file path and the index.
    /// </summary>
    /// <param name="filePath">The path which to be split.</param>
    /// <returns>A tuple containing the cleaned filepath and the <see cref="IconIndex"/> object containing the icon indicies.</returns>
    (string newFilePath, IconIndex? iconIndex) SplitIconIndex(string filePath);
}