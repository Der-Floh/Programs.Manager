using ProgramInfos.Manager.Reg.Data;
using ProgramInfos.Manager.Reg.Extensions;

namespace ProgramInfos.Manager.Reg.Service.IconLoader;

///<inheritdoc cref="IIconFinderService"/>
public sealed class IconFinderService : IIconFinderService
{
    /// <inheritdoc/>
    public string? GetIconPath(ProgramInfoData programInfoData)
    {
        //if (!string.IsNullOrEmpty(programInfoData.DisplayName) && programInfoData.DisplayName.ToLower().Contains("7-zip"))
        //{ }

        string? iconPath = null;

        try
        {
            if (!string.IsNullOrEmpty(programInfoData.DisplayIcon))
                iconPath = GetIconPathFromDisplayIconPath(programInfoData.DisplayIcon);
            if (iconPath is null && !string.IsNullOrEmpty(programInfoData.DisplayName))
            {
                if (!string.IsNullOrEmpty(programInfoData.Id) && programInfoData.Id.StartsWith('{') && programInfoData.Id.EndsWith('}'))
                    iconPath = GetIconPathFromWindowsInstallerCache(programInfoData.Id, programInfoData.DisplayName);
                if (iconPath is null && !string.IsNullOrEmpty(programInfoData.InstallLocation))
                    iconPath = GetIconPathFromAppDirectory(programInfoData.InstallLocation, programInfoData.DisplayName);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return iconPath;
    }

    private string? GetIconPathFromDisplayIconPath(string displayIcon)
    {
        string? iconPath = null;
        var extension = Path.GetExtension(displayIcon).ToLower();
        if (extension.Contains(".ico") || extension.Contains(".exe") || string.IsNullOrEmpty(extension))
        {
            (var iconPathTemp, var iconIndex) = SplitIconIndex(displayIcon);
            iconPathTemp = iconPathTemp.Trim('"');
            if (File.Exists(iconPathTemp))
                iconPath = iconPathTemp;
        }
        else if (extension.Contains(".jpg") || extension.Contains(".jpeg") || extension.Contains(".png"))
        {
            if (File.Exists(displayIcon))
                iconPath = displayIcon;
        }
        return iconPath;
    }

    private static string? GetIconPathFromAppDirectory(string installLocation, string displayName)
    {
        if (Directory.Exists(installLocation))
        {
            var files = Directory.EnumerateFiles(installLocation, "*.exe", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (Path.GetFileNameWithoutExtension(file).ContainsGeneralized(displayName))
                {
                    return file;
                }
            }
        }
        return null;
    }

    private static string? GetIconPathFromWindowsInstallerCache(string guid, string displayName)
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

        return !string.IsNullOrEmpty(iconPath) && File.Exists(iconPath) ? iconPath : null;
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

    public static (string newFilePath, IconIndex? iconIndex) SplitIconIndex(string filePath)
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
