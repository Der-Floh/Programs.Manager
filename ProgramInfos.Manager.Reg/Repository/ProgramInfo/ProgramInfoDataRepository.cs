using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Events;
using ProgramInfos.Manager.Abstractions.Service;
using ProgramInfos.Manager.Reg.Data;
using ProgramInfos.Manager.Reg.Service.CRegistry;
using System.Globalization;

namespace ProgramInfos.Manager.Reg.Repository.ProgramInfo;

/// <inheritdoc cref="IProgramInfoDataRepository"/>
public sealed class ProgramInfoDataRepository : IProgramInfoDataRepository
{
    private const string LMUninstallLocation64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string LMUninstallLocation32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string CUninstallLocation64 = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string CUninstallLocation32 = @"HKEY_CURRENT_USER\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";

    private readonly IProgramInfoDataSourceService _programInfoService;
    private readonly IRegistryService _registryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoDataRepository"/> class with specified services.
    /// </summary>
    /// <param name="programInfoService">The <see cref="IProgramInfoDataSourceService"/> object for managing program information.</param>
    /// <param name="registryService">The <see cref="IRegistryService"/> object for handling registry operations.</param>
    public ProgramInfoDataRepository(IProgramInfoDataSourceService programInfoService, IRegistryService registryService)
    {
        _programInfoService = programInfoService;
        _registryService = registryService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IProgramInfoData>> GetAll(ProgramInfoDataReceivedEvent? OnProgramInfoDataReceived = null)
    {
        var tasks = new List<Task<IEnumerable<ProgramInfoData>>>();
        foreach (ProgramInfoLocation location in Enum.GetValues(typeof(ProgramInfoLocation)))
        {
            tasks.Add(Task.Run(() => GetAllFromLocation(location, OnProgramInfoDataReceived)));
        }

        var programInfos = (await Task.WhenAll(tasks)).SelectMany(x => x);
        var programInfosNoDuplicates = FuseAllDuplicates(programInfos);

        return programInfosNoDuplicates;
    }

    /// <summary>
    /// Gets all program information from specified registry location.
    /// </summary>
    /// <param name="location">The registry location.</param>
    /// <param name="OnProgramInfoDataReceived">The <see cref="ProgramInfoDataReceivedEvent"/> event to be triggered when a program information is received.</param>
    /// <returns>An IEnumerable of <see cref="ProgramInfoData"/>.</returns>
    private IEnumerable<ProgramInfoData> GetAllFromLocation(ProgramInfoLocation location, ProgramInfoDataReceivedEvent? OnProgramInfoDataReceived = null)
    {
        string? locationPath = null;
        switch (location)
        {
            case ProgramInfoLocation.LocalMachineUninstallLocation64:
                locationPath = LMUninstallLocation64;
                break;
            case ProgramInfoLocation.LocalMachineUninstallLocation32:
                locationPath = LMUninstallLocation32;
                break;
            case ProgramInfoLocation.CurrentUserUninstallLocation64:
                locationPath = CUninstallLocation64;
                break;
            case ProgramInfoLocation.CurrentUserUninstallLocation32:
                locationPath = CUninstallLocation32;
                break;
        }

        if (string.IsNullOrEmpty(locationPath))
            return [];

        var programInfos = new List<ProgramInfoData>();
        var programKeyNames = _registryService.GetSubKeyNames(locationPath)?.Where(x => !string.IsNullOrEmpty(x)) ?? [];
        foreach (var programKeyName in programKeyNames)
        {
            var regPath = Path.Combine(locationPath, programKeyName);
            try
            {
                var programInfoData = WindowsRegistry.Serializer.RegistrySerializer.Deserialize<ProgramInfoData>(regPath);
                if (programInfoData is null)
                    continue;

                programInfoData.Id = programKeyName;
                programInfoData.RegKey = regPath;

                FillRegistryIgnoreProperties(ref programInfoData);

                programInfos.Add(programInfoData);
                OnProgramInfoDataReceived?.Invoke(this, new ProgramInfoDataEventArgs { ProgramInfoData = programInfoData });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        return programInfos;
    }

    /// <summary>
    /// Fills the properties of <see cref="ProgramInfoData"/> that were ignored during registry deserialization.
    /// </summary>
    /// <param name="programInfo">The <see cref="ProgramInfoData"/> to be filled.</param>
    private void FillRegistryIgnoreProperties(ref ProgramInfoData programInfo)
    {
        FillCultureInfo(ref programInfo);
        FillDisplayIconPath(ref programInfo);
    }

    /// <summary>
    /// Fills the <see cref="ProgramInfoData.CultureInfo"/> property.
    /// </summary>
    /// <param name="programInfo">The <see cref="ProgramInfoData"/> to be filled.</param>
    private static void FillCultureInfo(ref ProgramInfoData programInfoData)
    {
        if (!string.IsNullOrEmpty(programInfoData.Language) && int.TryParse(programInfoData.Language, out var lcid))
        {
            if (lcid > 0)
            {
                try
                {
                    programInfoData.CultureInfo = new CultureInfo(lcid);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }

    /// <summary>
    /// Fills the <see cref="ProgramInfoData.DisplayIconPath"/> property.
    /// </summary>
    /// <param name="programInfo">The <see cref="ProgramInfoData"/> to be filled.</param>
    private void FillDisplayIconPath(ref ProgramInfoData programInfoData)
    {
        try
        {
            programInfoData.DisplayIconInfo = _programInfoService.GetIconInfo(programInfoData);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// Fuses all duplicate program information entries in the specified collection of <see cref="ProgramInfoData"/>.
    /// </summary>
    /// <param name="programInfos">An IEnumerable of <see cref="ProgramInfoData"/>.</param>
    /// <returns>An IEnumerable of <see cref="ProgramInfoData"/> where duplicate entries have been fused.</returns>
    private static IEnumerable<ProgramInfoData> FuseAllDuplicates(IEnumerable<ProgramInfoData> programInfos)
    {
        var programInfosNoDuplicates = new List<ProgramInfoData>();
        var checkedDisplayNames = new List<string>();
        foreach (var programInfo in programInfos)
        {
            if (string.IsNullOrEmpty(programInfo.DisplayName) || checkedDisplayNames.Contains(programInfo.DisplayName))
                continue;
            var filtered = programInfos.Where(x => x.DisplayName == programInfo.DisplayName);
            if (filtered.Count() > 1)
            {
                var programRegInfoNoDuplicates = FuseDuplicates(filtered);
                if (programRegInfoNoDuplicates is not null)
                    programInfosNoDuplicates.Add(programRegInfoNoDuplicates);
            }
            else
            {
                programInfosNoDuplicates.Add(programInfo);
            }

            checkedDisplayNames.Add(programInfo.DisplayName);
        }
        return programInfosNoDuplicates;
    }

    /// <summary>
    /// Fuses all duplicates of a program information in the specified collection of <see cref="ProgramInfoData"/>.
    /// </summary>
    /// <param name="programInfos">An IEnumerable of <see cref="ProgramInfoData"/>.</param>
    /// <returns>A <see cref="ProgramInfoData"/> where duplicate entries have been fused.</returns>
    private static ProgramInfoData? FuseDuplicates(IEnumerable<ProgramInfoData> programInfos)
    {
        if (!programInfos.Any())
            return null;
        var programInfoToFuse = programInfos.First();
        var programInfoToFuseRegKeyDirectory = Path.GetDirectoryName(programInfoToFuse.RegKey);
        var filteredProgramInfos = programInfos.Where(x => Path.GetDirectoryName(x.RegKey) != programInfoToFuseRegKeyDirectory);
        foreach (var programInfo in filteredProgramInfos)
        {
            foreach (var property in programInfo.GetType().GetProperties())
            {
                var value = property.GetValue(programInfoToFuse);
                var newValue = property.GetValue(programInfo);
                if ((value is int intValue && intValue == -1) || (value is long longValue && longValue == -1) || (value is string stringValue && string.IsNullOrEmpty(stringValue)) || (value is null))
                    property.SetValue(programInfoToFuse, newValue);
            }
        }
        return programInfoToFuse;
    }
}
