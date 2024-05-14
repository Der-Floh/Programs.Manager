using Programs.Manager.Common.Win.Data;
using Programs.Manager.Common.Win.Repository;
using Programs.Manager.Common.Win.Service;
using Programs.Manager.Reader.Win.Data;
using Programs.Manager.Reader.Win.Service;

namespace Programs.Manager.Reader.Win.Repository.ProgramInfo;

///<inheritdoc cref="IProgramInfoDataRepository"/>
public sealed class ProgramInfoDataRepository : IProgramInfoDataRepository
{
    private const string LMUninstallLocation64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string LMUninstallLocation32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string CUninstallLocation64 = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string CUninstallLocation32 = @"HKEY_CURRENT_USER\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";

    private readonly IProgramInfoService _programInfoService;
    private readonly IWindowsLanguageService _windowsLanguageService;
    private readonly IRegistryService _registryService;
    private readonly IEnumerable<WindowsLanguageData>? _languages;
    public event ProgramInfoDataReceivedEvent OnProgramInfoDataReceived;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoDataRepository"/> class with specified services.
    /// </summary>
    /// <param name="programInfoService">The <see cref="IProgramInfoService"/> object for managing program information.</param>
    /// <param name="windowsLanguageService">The <see cref="IWindowsLanguageService"/> object for handling Windows language data.</param>
    public ProgramInfoDataRepository(IProgramInfoService programInfoService, IWindowsLanguageService windowsLanguageService, IRegistryService registryService)
    {
        _programInfoService = programInfoService;
        _windowsLanguageService = windowsLanguageService;
        _registryService = registryService;
        _languages = _windowsLanguageService.GetAll();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoDataRepository"/> class.
    /// </summary>
    public ProgramInfoDataRepository()
    {
        _programInfoService = new ProgramInfoService();
        _windowsLanguageService = new WindowsLanguageService();
        _registryService = new RegistryService();
        _languages = _windowsLanguageService.GetAll();
    }

    public IEnumerable<ProgramInfoData> GetAll()
    {
        var programInfosAll = new List<ProgramInfoData>();
        foreach (ProgramInfoLocation location in Enum.GetValues(typeof(ProgramInfoLocation)))
        {
            var programInfosLocation = GetAllFromLocation(location);
            programInfosAll.AddRange(programInfosLocation);
        }

        var programInfosWName = programInfosAll.Where(x => !string.IsNullOrEmpty(x.DisplayName));
        var programInfosNoDuplicates = FuseAllDuplicates(programInfosWName);
        return programInfosNoDuplicates;
    }

    private IEnumerable<ProgramInfoData> GetAllFromLocation(ProgramInfoLocation location = ProgramInfoLocation.LocalMachineUninstallLocation64)
    {
        var locationPath = LMUninstallLocation64;
        switch (location)
        {
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

        var programInfos = new List<ProgramInfoData>();
        var programKeyNames = _registryService.GetSubKeyNames(locationPath)?.Where(x => !string.IsNullOrEmpty(x)) ?? Array.Empty<string>();
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
