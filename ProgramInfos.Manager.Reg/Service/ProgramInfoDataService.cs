using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Service;
using ProgramInfos.Manager.Reg.Data;
using ProgramInfos.Manager.Reg.Service.CRegistry;
using ProgramInfos.Manager.Reg.Service.IconLoader;
using System.Diagnostics;

namespace ProgramInfos.Manager.Reg.Service;

/// <inheritdoc cref="IProgramInfoDataSourceService"/>
public class ProgramInfoDataService : IProgramInfoDataSourceService
{
    private const string CmdFileName = "cmd.exe";
    private const string ExplorerFileName = "explorer.exe";
    private const string RunAsAdminVerb = "runas";
    private readonly IRegistryService _registryService;
    private readonly IIconFinderService _iconLoaderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoService"/> class.
    /// </summary>
    /// <param name="registryService">The <see cref="IRegistryService"/> object to use for registry operations.</param>
    /// <param name="iconLoaderService">The <see cref="IIconFinderService"/> object to use for icon operations.</param>
    public ProgramInfoDataService(IRegistryService registryService, IIconFinderService iconLoaderService)
    {
        _registryService = registryService;
        _iconLoaderService = iconLoaderService;
    }

    /// <inheritdoc/>
    public bool IsResponsible(IProgramInfoData programInfoData) => programInfoData.SourceKey == ProgramInfoData.SourceKeyName;

    /// <inheritdoc/>
    public async Task<bool> Uninstall(IProgramInfoData programInfoData, bool quiet = false)
    {
        var arguments = programInfoData.UninstallString;
        if (quiet)
            arguments = programInfoData.QuietUninstallString;

        return await RunProcess(CmdFileName, arguments, true);
    }

    /// <inheritdoc/>
    public async Task<bool> Modify(IProgramInfoData programInfoData, string? additionalArguments = null)
    {
        var arguments = programInfoData.ModifyPath;
        if (!string.IsNullOrEmpty(additionalArguments))
            arguments += " " + additionalArguments;

        return await RunProcess(CmdFileName, arguments, true);
    }

    /// <inheritdoc/>
    public void OpenLocation(IProgramInfoData programInfoData)
    {
        if (!string.IsNullOrEmpty(programInfoData.InstallLocation))
        {
            Process.Start(ExplorerFileName, programInfoData.InstallLocation);
        }
    }

    /// <inheritdoc/>
    public void OpenLocationSource(IProgramInfoData programInfoData) => _registryService.OpenAt(programInfoData.RegKey);

    /// <inheritdoc/>
    public string? GetIconPath(IProgramInfoData programInfoData) => _iconLoaderService.GetIconPath((ProgramInfoData)programInfoData);

    /// <inheritdoc/>
    public void FetchFallbackProperties(IProgramInfoData programInfoData)
    {
        if (programInfoData.EstimatedSize == -1 && !string.IsNullOrEmpty(programInfoData.InstallLocation))
        {
            programInfoData.EstimatedSize = GetProgramSize(programInfoData.InstallLocation);
        }
    }

    /// <inheritdoc/>
    public void UpdateFromDifferent(IProgramInfoData programInfoDataToUpdate, IProgramInfoData programInfoDataToCopy)
    {
        foreach (var property in programInfoDataToUpdate.GetType().GetProperties())
        {
            var originalValue = property.GetValue(programInfoDataToUpdate);
            var updateValue = property.GetValue(programInfoDataToCopy);
            if (updateValue is not null && originalValue != updateValue)
                property.SetValue(programInfoDataToUpdate, property.GetValue(programInfoDataToCopy));
        }
    }

    private static long GetProgramSize(string installLocation)
    {
        if (Directory.Exists(installLocation))
        {
            try
            {
                var directoryInfo = new DirectoryInfo(installLocation);
                long totalSize = 0;
                var fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
                foreach (var fileInfo in fileInfos)
                {
                    totalSize += fileInfo.Length;
                }
                return totalSize;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        return -1;
    }

    private static async Task<bool> RunProcess(string processName) => await RunProcess(processName, null, false);
    private static async Task<bool> RunProcess(string processName, string? arguments) => await RunProcess(processName, arguments, false);
    private static async Task<bool> RunProcess(string processName, bool runAsAdmin) => await RunProcess(processName, null, runAsAdmin);
    private static async Task<bool> RunProcess(string processName, string? arguments, bool runAsAdmin)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = processName,
            Arguments = arguments,
            UseShellExecute = true,
            CreateNoWindow = true,
        };

        if (runAsAdmin)
        {
            startInfo.Verb = RunAsAdminVerb;
        }

        var process = new Process { StartInfo = startInfo };

        try
        {
            process.Start();
            await process.WaitForExitAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
