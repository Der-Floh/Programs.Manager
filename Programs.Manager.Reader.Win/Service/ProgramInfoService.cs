using Programs.Manager.Common.Win.Data;
using Programs.Manager.Common.Win.Service;
using System.Diagnostics;
using System.Drawing;

namespace Programs.Manager.Reader.Win.Service;

///<inheritdoc cref="IProgramInfoService"/>
public sealed class ProgramInfoService : IProgramInfoService
{
    private const string CmdFileName = "cmd.exe";
    private const string RunAsAdminVerb = "runas";
    private readonly IRegistryService _registryService;
    private readonly IIconLoaderService _iconLoaderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoService"/> class.
    /// </summary>
    /// <param name="registryService">The <see cref="IRegistryService"/> object to use for registry operations.</param>
    /// <param name="iconLoaderService">The <see cref="IIconLoaderService"/> object to use for icon operations.</param>
    public ProgramInfoService(IRegistryService registryService, IIconLoaderService iconLoaderService)
    {
        _registryService = registryService;
        _iconLoaderService = iconLoaderService;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoService"/> class.
    /// </summary>
    public ProgramInfoService()
    {
        _registryService = new RegistryService();
        _iconLoaderService = new IconLoaderService();
    }

    public void FetchFallbackProperties(ProgramInfoData programInfoData)
    {
        if (programInfoData.EstimatedSize == -1 && !string.IsNullOrEmpty(programInfoData.InstallLocation))
        {
            try
            {
                var directoryInfo = new DirectoryInfo(programInfoData.InstallLocation);
                long totalSize = 0;
                var fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
                foreach (var fileInfo in fileInfos)
                    totalSize += fileInfo.Length;
                programInfoData.EstimatedSize = totalSize;
            }
            catch { }
        }
    }

    public void UpdateFromDifferent(ProgramInfoData programInfoData, ProgramInfoData programInfoDataToCopy)
    {
        foreach (var property in programInfoData.GetType().GetProperties())
        {
            var originalValue = property.GetValue(programInfoData);
            var updateValue = property.GetValue(programInfoDataToCopy);
            if (updateValue is not null && originalValue != updateValue)
                property.SetValue(programInfoData, property.GetValue(programInfoDataToCopy));
        }
    }

    public async Task<bool> Uninstall(ProgramInfoData programInfoData, bool quiet = false)
    {
        var arguments = programInfoData.UninstallString;
        if (quiet)
            arguments = programInfoData.QuietUninstallString;

        return await RunProcess(CmdFileName, arguments);
    }

    public async Task<bool> Modify(ProgramInfoData programInfoData, string? additionalArguments = null)
    {
        var arguments = programInfoData.ModifyPath;
        if (!string.IsNullOrEmpty(additionalArguments))
            arguments += " " + additionalArguments;

        return await RunProcess(CmdFileName, arguments);
    }

    public bool OpenRegistry(ProgramInfoData programInfoData) => !string.IsNullOrEmpty(programInfoData.RegKey) && _registryService.OpenAt(programInfoData.RegKey);

    private static async Task<bool> RunProcess(string processName, string? arguments = null)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = processName,
            Arguments = arguments,
            UseShellExecute = true,
            CreateNoWindow = true,
            Verb = RunAsAdminVerb,
        };

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

    public Task<bool> LoadIcon(ProgramInfoData programInfoData) => throw new NotImplementedException();

    public Bitmap? GetIcon(ProgramInfoData programInfoData)
    {
        try
        {
            return _iconLoaderService.GetIcon(programInfoData);
        }
        catch { return null; }
    }
}
