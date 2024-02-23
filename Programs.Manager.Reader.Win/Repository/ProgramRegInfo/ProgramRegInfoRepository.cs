using Programs.Manager.Reader.Win.Data;
using Programs.Manager.Reader.Win.Extensions;
using Programs.Manager.Reader.Win.Service.ProgramRegInfo;
using Programs.Manager.Reader.Win.Utility;
using System.Reflection;

namespace Programs.Manager.Reader.Win.Repository.ProgramRegInfo;

///<inheritdoc cref="IProgramInfoRepository"/>
public sealed class ProgramRegInfoRepository : IProgramRegInfoRepository
{
    private const string LMUninstallLocation64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string LMUninstallLocation32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string CUninstallLocation64 = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
    private const string CUninstallLocation32 = @"HKEY_CURRENT_USER\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
    private readonly IProgramRegInfoService _programRegInfoService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramRegInfoRepository"/> class with specified services.
    /// </summary>
    /// <param name="programRegInfoService">The <see cref="IProgramRegInfoService"/> object to use for ProgramRegInfo operations.</param>
    public ProgramRegInfoRepository(IProgramRegInfoService programRegInfoService)
    {
        _programRegInfoService = programRegInfoService;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramRegInfoRepository"/> class.
    /// </summary>
    public ProgramRegInfoRepository()
    {
        _programRegInfoService = new ProgramRegInfoService();
    }

    public ProgramRegInfoData Get(Dictionary<string, object?> keys, string regKey)
    {
        var id = regKey[(regKey.LastIndexOf('\\') + 1)..];
        var programInfo = new ProgramRegInfoData(_programRegInfoService) { RegKey = regKey, Id = id };
        foreach (var key in keys.Keys)
        {
            var value = keys[key]?.ToString()?.GetReadable();
            if (!string.IsNullOrEmpty(value) && value.StartsWith('"') && value.EndsWith('"'))
                value = value.Trim('"');
            programInfo.GetType().GetProperty(key)?.SetValue(programInfo, value);
        }
        return programInfo;
    }

    public IEnumerable<ProgramRegInfoData> GetAll()
    {
        var programRegInfosAll = new List<ProgramRegInfoData>();
        foreach (ProgramInfoLocation location in Enum.GetValues(typeof(ProgramInfoLocation)))
        {
            IEnumerable<ProgramRegInfoData> programInfosLocation = GetAllFromLocation(location);
            programRegInfosAll.AddRange(programInfosLocation);
        }

        IEnumerable<ProgramRegInfoData> programRegInfosWName = programRegInfosAll.Where(x => !string.IsNullOrEmpty(x.DisplayName));
        IEnumerable<ProgramRegInfoData> programRegInfosNoDuplicates = FuseAllDuplicates(programRegInfosWName);

        return programRegInfosNoDuplicates;
    }

    private IEnumerable<ProgramRegInfoData> GetAllFromLocation(ProgramInfoLocation location = ProgramInfoLocation.LocalMachineUninstallLocation64)
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

        var programInfos = new List<ProgramRegInfoData>();
        var programs = RegistryHelper.GetRegKeys(locationPath);
        foreach (var program in programs)
        {
            Dictionary<string, object?> keys = RegistryHelper.GetRegKeyValues(locationPath + program);
            programInfos.Add(Get(keys, locationPath + program));
        }
        return programInfos;
    }

    private static IEnumerable<ProgramRegInfoData> FuseAllDuplicates(IEnumerable<ProgramRegInfoData> programRegInfos)
    {
        var programInfosNoDuplicates = new List<ProgramRegInfoData>();
        var checkedDisplayNames = new List<string>();
        foreach (ProgramRegInfoData program in programRegInfos)
        {
            if (checkedDisplayNames.Contains(program.DisplayName))
                continue;
            ProgramRegInfoData[] filtered = programRegInfos.Where(x => x.DisplayName == program.DisplayName).ToArray();
            if (filtered.Length > 1)
            {
                ProgramRegInfoData? programRegInfoNoDuplicates = FuseDuplicates(filtered);
                if (programRegInfoNoDuplicates is not null)
                    programInfosNoDuplicates.Add(programRegInfoNoDuplicates);
            }
            else
            {
                programInfosNoDuplicates.Add(program);
            }

            checkedDisplayNames.Add(program.DisplayName);
        }
        return programInfosNoDuplicates.ToArray();
    }

    private static ProgramRegInfoData? FuseDuplicates(IEnumerable<ProgramRegInfoData> programRegInfos)
    {
        if (!programRegInfos.Any())
            return null;
        ProgramRegInfoData programRegInfo = programRegInfos.First();
        foreach (ProgramRegInfoData program in programRegInfos)
        {
            foreach (PropertyInfo property in program.GetType().GetProperties())
            {
                if (string.IsNullOrEmpty(property.GetValue(programRegInfo)?.ToString()))
                    property.SetValue(programRegInfo, property.GetValue(program));
            }
        }
        return programRegInfo;
    }
}
