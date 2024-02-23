using Programs.Manager.Reader.Win.Data;
using Programs.Manager.Reader.Win.Repository.ProgramRegInfo;
using Programs.Manager.Reader.Win.Service.ProgramInfo;
using Programs.Manager.Reader.Win.Service.WindowsLanguage;

namespace Programs.Manager.Reader.Win.Repository.ProgramInfo;

///<inheritdoc cref="IProgramInfoRepository"/>
public sealed class ProgramInfoRepository : IProgramInfoRepository
{
    private readonly IProgramInfoService _programInfoService;
    private readonly IProgramRegInfoRepository _programRegInfoRepository;
    private readonly IWindowsLanguageService _windowsLanguageService;

    private readonly IEnumerable<WindowsLanguageData>? _languages;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoRepository"/> class with specified services.
    /// </summary>
    /// <param name="programInfoService">The <see cref="IProgramInfoService"/> object for managing program information.</param>
    /// <param name="programRegInfoRepository">The <see cref="IProgramRegInfoRepository"/> object for accessing registry information about programs.</param>
    /// <param name="windowsLanguageService">The <see cref="IWindowsLanguageService"/> object for handling Windows language data.</param>
    public ProgramInfoRepository(IProgramInfoService programInfoService, IProgramRegInfoRepository programRegInfoRepository, IWindowsLanguageService windowsLanguageService)
    {
        _programInfoService = programInfoService;
        _programRegInfoRepository = programRegInfoRepository;
        _windowsLanguageService = windowsLanguageService;

        _languages = _windowsLanguageService.GetAll();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoRepository"/> class.
    /// </summary>
    public ProgramInfoRepository()
    {
        _programInfoService = new ProgramInfoService();
        _programRegInfoRepository = new ProgramRegInfoRepository();
        _windowsLanguageService = new WindowsLanguageService();

        _languages = _windowsLanguageService.GetAll();
    }

    public IEnumerable<ProgramInfoData> GetAll(Action<ProgramRegInfoData>? action = null)
    {
        IEnumerable<ProgramRegInfoData> programRegInfos = _programRegInfoRepository.GetAll();

        var programInfos = new List<ProgramInfoData>();
        foreach (ProgramRegInfoData program in programRegInfos)
        {
            try
            {
                action?.Invoke(program);
                program.FetchFallbackValues();
                ProgramInfoData programInfo = FromProgramRegInfo(program);
                programInfos.Add(programInfo);
            }
            catch { }
        }

        return programInfos;
    }

    private ProgramInfoData FromProgramRegInfo(ProgramRegInfoData regInfo)
    {
        var programInfo = new ProgramInfoData(_programInfoService)
        {
            RegKey = regInfo.RegKey,

            Id = regInfo.Id,
            Comments = regInfo.Comments,
            Contact = regInfo.Contact,

            DisplayIcon = regInfo.GetIcon(),
            DisplayName = regInfo.DisplayName,
            DisplayVersion = regInfo.DisplayVersion,

            HelpLink = regInfo.HelpLink,
            HelpTelephone = regInfo.HelpTelephone,
            InstallDate = regInfo.InstallDate,
            InstallLocation = string.IsNullOrEmpty(regInfo.InstallDir) ? regInfo.InstallLocation : regInfo.InstallDir,
            InstallSource = regInfo.InstallSource,

            ModifyPath = regInfo.ModifyPath,
            NoModify = regInfo.NoModify == "1",
            NoRemove = regInfo.NoRemove == "1",
            NoRepair = regInfo.NoRepair == "1",

            Publisher = regInfo.Publisher,
            Readme = regInfo.Readme,
            SystemComponent = regInfo.SystemComponent == "1",

            QuietUninstallString = regInfo.QuietUninstallString,
            UninstallString = regInfo.UninstallString,
            UrlInfoAbout = regInfo.UrlInfoAbout,
            UrlUpdateInfo = regInfo.UrlUpdateInfo,

            WindowsInstaller = regInfo.WindowsInstaller == "1",
        };

        if (!string.IsNullOrEmpty(regInfo.EstimatedSize) && long.TryParse(regInfo.EstimatedSize, out var size))
            programInfo.EstimatedSize = size * 1024;

        if (!string.IsNullOrEmpty(regInfo.Language))
        {
            WindowsLanguageData? language = _languages?.FirstOrDefault(x => x.WindowsCodeDecimal.ToString() == regInfo.Language);
            programInfo.Language = language?.LCIDCode;
        }

        if (!string.IsNullOrEmpty(regInfo.VersionMajor) && long.TryParse(regInfo.VersionMajor, out var versionMajor))
            programInfo.VersionMajor = versionMajor;

        if (!string.IsNullOrEmpty(regInfo.VersionMinor) && long.TryParse(regInfo.VersionMinor, out var versionMinor))
            programInfo.VersionMinor = versionMinor;

        return programInfo;
    }
}
