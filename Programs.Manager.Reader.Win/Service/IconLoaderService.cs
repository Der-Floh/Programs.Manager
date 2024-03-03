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
        if (programRegInfoData.DisplayName.ToLower().Contains("beyond compare"))
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

        return icon;
    }

    private Bitmap? GetIconFromDisplayIconPath(string displayIcon)
    {
        Bitmap? icon = null;
        var extension = Path.GetExtension(displayIcon).ToLower();
        if (extension.Contains(".ico") || extension.Contains(".exe"))
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
        IEnumerable<string> files = Directory.EnumerateFiles(installLocation, "*.exe", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            if (Path.GetFileNameWithoutExtension(file).ContainsGeneralized(displayName))
            {
                var iconData = _iconReader.Read(file);
                var iconBytes = iconData?.GetImage(iconData.PreferredImageIndex());
                if (iconBytes is null)
                    continue;
                var icon = new Bitmap(new MemoryStream(iconBytes));
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

        if (iconPath is not null && File.Exists(iconPath))
        {
            try
            {
                var iconData = _iconReader.Read(iconPath);
                var iconBytes = iconData?.GetImage(iconData.PreferredImageIndex());
                if (iconBytes is null)
                    return null;
                var icon = new Bitmap(new MemoryStream(iconBytes));
                return icon;
            }
            catch { return null; }
        }
        else
        {
            return null;
        }
    }

    private static string? GetIconPathFromDirectoryIco(string directoryPath, string displayName)
    {
        FileInfo[] icoFiles = new DirectoryInfo(directoryPath).GetFiles("*.ico").OrderByDescending(x => x.Length).ToArray();
        return GetIconFileFromFiles(icoFiles, displayName);
    }

    private static string? GetIconPathFromDirectoryExe(string directoryPath, string displayName)
    {
        FileInfo[] exeFiles = new DirectoryInfo(directoryPath).GetFiles("*.exe").OrderByDescending(x => x.Length).ToArray();
        return GetIconFileFromFiles(exeFiles, displayName);
    }

    private static string? GetIconPathFromDirectoryNoExt(string directoryPath, string displayName)
    {
        FileInfo[] allFiles = new DirectoryInfo(directoryPath).GetFiles();
        FileInfo[] filesWithoutExt = allFiles.Where(x => string.IsNullOrEmpty(x.Extension)).ToArray();
        return GetIconFileFromFiles(filesWithoutExt, displayName);
    }

    private static string? GetIconFileFromFiles(FileInfo[] files, string displayName)
    {
        if (files.Length != 0)
        {
            FileInfo? file = FindSimilarDisplayName(files, displayName);
            file ??= files[0];
            return file.FullName;
        }
        return null;
    }

    private static FileInfo? FindSimilarDisplayName(FileInfo[] files, string displayName)
    {
        FileInfo? similarFileWithName = null;
        FileInfo? similarFileWithIcon = null;
        foreach (FileInfo file in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FullName);
            if (fileName.ContainsGeneralized(displayName))
            {
                similarFileWithName = file;
                break;
            }
            if (fileName.ContainsGeneralized("icon"))
                similarFileWithIcon = file;
        }

        return similarFileWithName is null ? similarFileWithIcon : similarFileWithName;
    }

    public Bitmap? GetIconFromFile(string iconPath, IconIndex? iconIndex = null)
    {
        var iconData = _iconReader.Read(iconPath);
        byte[]? iconBytes = null;
        if (iconIndex is not null)
        {
            if (iconIndex.Index >= 0 && iconIndex.Index < iconData?.ImageReferences.Count)
            {
                iconBytes = iconData?.GetImage(iconIndex.Index);
            }
            else if (!string.IsNullOrEmpty(iconIndex.GroupName))
            {
                var iconGroup = iconData?.Groups.FirstOrDefault(x => x.Name == iconIndex.GroupName);
                if (iconGroup is not null)
                    iconBytes = iconData?.GetImage(iconData.PreferredImageIndex(iconGroup.Name));
            }
        }

        iconBytes ??= iconData?.GetImage(iconData.PreferredImageIndex());

        if (iconBytes is null)
            return null;
        var icon = new Bitmap(new MemoryStream(iconBytes));
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
