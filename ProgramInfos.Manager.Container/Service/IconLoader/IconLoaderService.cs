using Ico.Reader;
using ProgramInfos.Manager.Abstractions.Data;
using System.Text.RegularExpressions;

namespace ProgramInfos.Manager.Container.Service.IconLoader;

/// <summary>
/// Class for loading an Icon from an <see cref="IProgramInfoData"/>.
/// </summary>
public partial class IconLoaderService : IIconLoaderService
{
    private readonly IcoReader _iconReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconLoaderService"/> class.
    /// </summary>
    public IconLoaderService()
    {
        _iconReader = new IcoReader();
    }

    /// <inheritdoc/>
    public MemoryStream? GetIcon(IProgramInfoData programInfoData) => GetIconFromFile(programInfoData.DisplayIconInfo);

    /// <summary>
    /// Loads an Icon from a file.
    /// </summary>
    /// <param name="iconPath">The path to the icon file.</param>
    /// <param name="iconIndex">Optional. The index of the icon inside the icon file.</param>
    /// <param name="groupName">Optional. The name of the icon group for which the index should be used.</param>
    /// <returns>A <see cref="MemoryStream"/> containing the image data.</returns>
    private MemoryStream? GetIconFromFile(IIconInfo? iconInfo)
    {
        if (string.IsNullOrEmpty(iconInfo?.Path))
            return null;

        var iconData = _iconReader.Read(iconInfo.Path);
        if (iconData is null)
            return null;

        byte[]? iconBytes = null;
        var index = 0;
        if (iconInfo.Index != -1)
        {
            if (iconInfo.Index >= 0 && iconInfo.Index < iconData.ImageReferences.Count)
            {
                if (string.IsNullOrEmpty(iconInfo.GroupName))
                    iconInfo.GroupName = FindImageRefGroupName(iconData, iconInfo.Index)!;
                index = iconData.PreferredImageIndex(iconInfo.GroupName);

                iconBytes = iconData.GetImage(index);
            }
            else if (!string.IsNullOrEmpty(iconInfo.GroupName))
            {
                var iconGroup = iconData.Groups.FirstOrDefault(x => x.Name == iconInfo.GroupName);
                if (iconGroup is not null)
                    iconBytes = iconData.GetImage(iconData.PreferredImageIndex(iconGroup.Name));
            }
        }

        var mainGroupName = FindMainGroupName(iconData);
        index = mainGroupName is null ? iconData.PreferredImageIndex() : iconData.PreferredImageIndex(mainGroupName);
        iconBytes ??= iconData.GetImage(index);

        return iconBytes is null ? null : new MemoryStream(iconBytes);
    }

    /// <summary>
    /// Finds the name of the group that contains the image with the given index.
    /// </summary>
    /// <param name="icoData">The <see cref="Ico.Reader.Data.IcoData"/> to search in.</param>
    /// <param name="imageRefIndex">The index of the image to find.</param>
    /// <returns>A string representing the name of the group that contains the image.</returns>
    private static string? FindImageRefGroupName(Ico.Reader.Data.IcoData icoData, int imageRefIndex)
    {
        var imageRef = icoData.ImageReferences[imageRefIndex];
        foreach (var group in icoData.Groups)
        {
            foreach (var directoryEntry in group.DirectoryEntries)
            {
                if (directoryEntry.RealImageOffset == imageRef.Offset)
                    return group.Name;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds the name of the group that contains the main icon.
    /// </summary>
    /// <param name="icoData">The <see cref="Ico.Reader.Data.IcoData"/> to search in.</param>
    /// <returns>A string representing the name of the group that contains the main icon.</returns>
    private static string? FindMainGroupName(Ico.Reader.Data.IcoData icoData)
    {
        foreach (var group in icoData.Groups)
        {
            if (ContainsGeneralized(group.Name, "main") || ContainsGeneralized(group.Name, "logo"))
                return group.Name;
        }
        return null;
    }

    /// <summary>
    /// Checks if two generalized strings contain each other.
    /// </summary>
    /// <param name="string1">The first string.</param>
    /// <param name="string2">The second string.</param>
    /// <returns>A boolean indicating whether the strings contain each other.</returns>
    private static bool ContainsGeneralized(string string1, string string2)
    {
        var generalized1 = GeneralizeRegex().Replace(string1, "").ToLower();
        var generalized2 = GeneralizeRegex().Replace(string2, "").ToLower();

        return generalized1.Contains(generalized2) || generalized2.Contains(generalized1);
    }

    [GeneratedRegex("\\s+")]
    private static partial Regex GeneralizeRegex();
}
