using Programs.Manager.Reader.Win.Data;
using Programs.Manager.Reader.Win.Service.EmbeddedResource;
using Programs.Manager.Reader.Win.Service.RegJump;
using System.Diagnostics;
using System.Reflection;

namespace Programs.Manager.Reader.Win.Service.ProgramInfo;

///<inheritdoc cref="IIconLoaderService"/>
public sealed class ProgramInfoService : IProgramInfoService
{
    private const string CmdFileName = "cmd.exe";
    private const string RunAsAdminVerb = "runas";
    private readonly IRegJumpService _regJumpService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoService"/> class.
    /// </summary>
    /// <param name="regJumpService">The <see cref="IRegJumpService"/> object to use for registry operations.</param>
    public ProgramInfoService(IRegJumpService regJumpService)
    {
        _regJumpService = regJumpService;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoService"/> class.
    /// </summary>
    public ProgramInfoService()
    {
        var _embeddedResourceService = new EmbeddedResourceService();
        _regJumpService = new RegJumpService(_embeddedResourceService);
    }

    public void UpdateFromDifferent(ProgramInfoData programInfoData, ProgramInfoData programInfoDataToCopy)
    {
        foreach (PropertyInfo property in programInfoData.GetType().GetProperties())
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

    public async Task<bool> OpenRegistry(ProgramInfoData programInfoData) => !string.IsNullOrEmpty(programInfoData.RegKey) && await _regJumpService.OpenAt(programInfoData.RegKey);

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
        catch { return false; }
    }
}
