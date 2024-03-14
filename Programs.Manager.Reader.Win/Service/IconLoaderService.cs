using Ico.Reader;
using Programs.Manager.Common.Win.Data;
using Programs.Manager.Common.Win.Service;
using Programs.Manager.Reader.Win.Extensions;
using System.Drawing;

namespace Programs.Manager.Reader.Win.Service;

///<inheritdoc cref="IIconLoaderService"/>
public sealed class IconLoaderService : IIconLoaderService
{
    private readonly IcoReader _iconReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconLoaderService"/> class.
    /// </summary>
    public IconLoaderService()
    {
        _iconReader = new IcoReader();
    }

    public Bitmap? GetIcon(ProgramRegInfoData programRegInfoData)
    {
        if (programRegInfoData.DisplayName.ToLower().Contains("snapdragon"))
        { }

        Bitmap? icon = null;
        if (!string.IsNullOrEmpty(programRegInfoData.DisplayIcon))
            icon = GetIconFromDisplayIconPath(programRegInfoData.DisplayIcon);
        if (icon is null && !string.IsNullOrEmpty(programRegInfoData.DisplayName))
        {
            if (!string.IsNullOrEmpty(programRegInfoData.Id) && programRegInfoData.Id.StartsWith('{') && programRegInfoData.Id.EndsWith('}'))
                icon = GetIconFromWindowsInstallerCache(programRegInfoData.Id, programRegInfoData.DisplayName);
            if (icon is null && !string.IsNullOrEmpty(programRegInfoData.InstallLocation))
                icon = GetIconFromAppDirectory(programRegInfoData.InstallLocation, programRegInfoData.DisplayName);
        }

        //if (icon is not null)
        //{
        //    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "IconsTest");
        //    if (!Directory.Exists(path))
        //        Directory.CreateDirectory(path);
        //    var invalidChars = Path.GetInvalidFileNameChars();
        //    var displayNameFile = new string(programRegInfoData.DisplayName.Where(ch => !invalidChars.Contains(ch) && ch >= 32).ToArray());
        //    var file = Path.Combine(path, displayNameFile + ".png");
        //    try
        //    {
        //        icon.Save(file);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (File.Exists(file))
        //            File.Delete(file);
        //        File.WriteAllText(Path.Combine(path, "_Error_" + programRegInfoData.DisplayName + ".txt"), ex.ToString());
        //    }
        //}

        return icon;
    }

    private Bitmap? GetIconFromDisplayIconPath(string displayIcon)
    {
        Bitmap? icon = null;
        var extension = Path.GetExtension(displayIcon).ToLower();
        if (extension.Contains(".ico") || extension.Contains(".exe") || string.IsNullOrEmpty(extension))
        {
            (var iconPath, var iconIndex) = SplitIconIndex(displayIcon);
            iconPath = iconPath.Trim('"');
            if (File.Exists(iconPath))
                icon = GetIconFromFile(iconPath, iconIndex);
        }
        else if (extension.Contains(".jpg") || extension.Contains(".jpeg") || extension.Contains(".png"))
        {
            if (File.Exists(displayIcon))
                icon = new Bitmap(displayIcon);
        }
        return icon;
    }

    private Bitmap? GetIconFromAppDirectory(string installLocation, string displayName)
    {
        var files = Directory.EnumerateFiles(installLocation, "*.exe", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (Path.GetFileNameWithoutExtension(file).ContainsGeneralized(displayName))
            {
                var icon = GetIconFromFile(file);
                if (icon is not null)
                    return icon;
            }
        }
        return null;
    }

    private Bitmap? GetIconFromWindowsInstallerCache(string guid, string displayName)
    {
        var windowsPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.System)) ?? string.Empty;
        var installerPath = Path.Combine(windowsPath, "Installer", guid);

        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var installerPathUser = Path.Combine(appDataPath, "Microsoft", "Installer", guid);

        string? iconPath = null;

        if (Directory.Exists(installerPath))
        {
            iconPath ??= GetIconPathFromDirectoryIco(installerPath, displayName);
            iconPath ??= GetIconPathFromDirectoryExe(installerPath, displayName);
            iconPath ??= GetIconPathFromDirectoryNoExt(installerPath, displayName);
        }

        if (Directory.Exists(installerPathUser))
        {
            iconPath ??= GetIconPathFromDirectoryIco(installerPathUser, displayName);
            iconPath ??= GetIconPathFromDirectoryExe(installerPathUser, displayName);
            iconPath ??= GetIconPathFromDirectoryNoExt(installerPathUser, displayName);
        }

        if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
            return GetIconFromFile(iconPath);
        else
            return null;
    }

    private static string? GetIconPathFromDirectoryIco(string directoryPath, string displayName)
    {
        var icoFiles = new DirectoryInfo(directoryPath).GetFiles("*.ico").OrderByDescending(x => x.Length).ToArray();
        return GetIconFileFromFiles(icoFiles, displayName);
    }

    private static string? GetIconPathFromDirectoryExe(string directoryPath, string displayName)
    {
        var exeFiles = new DirectoryInfo(directoryPath).GetFiles("*.exe").OrderByDescending(x => x.Length).ToArray();
        return GetIconFileFromFiles(exeFiles, displayName);
    }

    private static string? GetIconPathFromDirectoryNoExt(string directoryPath, string displayName)
    {
        var allFiles = new DirectoryInfo(directoryPath).GetFiles();
        var filesWithoutExt = allFiles.Where(x => string.IsNullOrEmpty(x.Extension)).ToArray();
        return GetIconFileFromFiles(filesWithoutExt, displayName);
    }

    private static string? GetIconFileFromFiles(FileInfo[] files, string displayName)
    {
        if (files.Length != 0)
        {
            var file = FindSimilarDisplayName(files, displayName);
            file ??= files[0];
            return file.FullName;
        }
        return null;
    }

    private static FileInfo? FindSimilarDisplayName(FileInfo[] files, string displayName)
    {
        FileInfo? similarFileWithName = null;
        FileInfo? similarFileWithIcon = null;
        foreach (var file in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FullName);
            if (fileName.ContainsGeneralized(displayName))
            {
                similarFileWithName = file;
                break;
            }
            if (fileName.ContainsGeneralized("icon") || fileName.ContainsGeneralized("logo"))
                similarFileWithIcon = file;
        }

        return similarFileWithName is null ? similarFileWithIcon : similarFileWithName;
    }

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

    private static string? FindMainGroupName(Ico.Reader.Data.IcoData icoData)
    {
        foreach (var group in icoData.Groups)
        {
            if (group.Name.ContainsGeneralized("main") || group.Name.ContainsGeneralized("logo"))
                return group.Name;
        }
        return null;
    }

    public Bitmap? GetIconFromFile(string iconPath, IconIndex? iconIndex = null)
    {
        var iconData = _iconReader.Read(iconPath);
        if (iconData is null)
            return null;

        byte[]? iconBytes = null;
        var index = 0;
        if (iconIndex is not null)
        {
            if (iconIndex.Index >= 0 && iconIndex.Index < iconData.ImageReferences.Count)
            {
                var groupName = FindImageRefGroupName(iconData, iconIndex.Index)!;
                index = iconData.PreferredImageIndex(groupName);

                iconBytes = iconData.GetImage(index);
            }
            else if (!string.IsNullOrEmpty(iconIndex.GroupName))
            {
                var iconGroup = iconData.Groups.FirstOrDefault(x => x.Name == iconIndex.GroupName);
                if (iconGroup is not null)
                    iconBytes = iconData.GetImage(iconData.PreferredImageIndex(iconGroup.Name));
            }
        }

        var mainGroupName = FindMainGroupName(iconData);
        if (mainGroupName is null)
            index = iconData.PreferredImageIndex();
        else
            index = iconData.PreferredImageIndex(mainGroupName);
        iconBytes ??= iconData.GetImage(index);

        if (iconBytes is null)
            return null;

        var icon = new Bitmap(new MemoryStream(iconBytes), true);

        return icon;
    }

    public string RemoveIconIndex(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        if (extension.Contains(','))
            extension = extension[..extension.LastIndexOf(',')];
        var extensionIndex = filePath.LastIndexOf(extension) + extension.Length - 1;
        var index = filePath.LastIndexOf(',');
        return index < extensionIndex ? filePath : filePath[..index];
    }

    public (string newFilePath, IconIndex? iconIndex) SplitIconIndex(string filePath)
    {
        var index = filePath.LastIndexOf(',');
        if (index == -1)
        {
            return (filePath, null);
        }
        else
        {
            var cleanedFilePath = filePath[..index];
            var success = int.TryParse(filePath.AsSpan(index + 1), out var parsedIndex);
            var iconIndex = new IconIndex();
            if (success)
            {
                if (parsedIndex < 0)
                    iconIndex.GroupName = Math.Abs(parsedIndex).ToString();
                else
                    iconIndex.Index = parsedIndex;
            }
            else
            {
                iconIndex.GroupName = filePath.AsSpan(index + 1).ToString();
            }
            return (cleanedFilePath, iconIndex);
        }
    }
}
