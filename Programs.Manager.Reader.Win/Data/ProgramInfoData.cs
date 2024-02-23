using Programs.Manager.Reader.Win.Service.ProgramInfo;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Programs.Manager.Reader.Win.Data;

/// <summary>
/// Contains information about a program.
/// </summary>
public class ProgramInfoData : INotifyPropertyChanged
{
    /// <summary>
    /// Unique identifier for the program. Is equivalent to the registry Program Guid
    /// </summary>
    public string Id { get => _id; set { if (_id == value) return; _id = value; OnPropertyChanged(); } }
    private string _id = string.Empty;

    /// <summary>
    /// Comments associated with the program, if available.
    /// </summary>
    public string? Comments { get => _comments; set { if (_comments == value) return; _comments = value; OnPropertyChanged(); } }
    private string? _comments;

    /// <summary>
    /// Contact information for the program's publisher or support.
    /// </summary>
    public string? Contact { get => _contact; set { if (_contact == value) return; _contact = value; OnPropertyChanged(); } }
    private string? _contact;

    /// <summary>
    /// Icon representing the program, if available.
    /// </summary>
    public Bitmap? DisplayIcon { get => _displayIcon; set { if (_displayIcon == value) return; _displayIcon = value; OnPropertyChanged(); } }
    private Bitmap? _displayIcon;

    /// <summary>
    /// Display name of the program as registered in the system.
    /// </summary>
    public string? DisplayName { get => _displayName; set { if (_displayName == value) return; _displayName = value; OnPropertyChanged(); } }
    private string? _displayName;

    /// <summary>
    /// Version of the program as displayed in the registry.
    /// </summary>
    public string? DisplayVersion { get => _displayVersion; set { if (_displayVersion == value) return; _displayVersion = value; OnPropertyChanged(); } }
    private string? _displayVersion;

    /// <summary>
    /// Estimated size of the program on disk, in bytes.
    /// </summary>
    public long EstimatedSize { get => _estimatedSize; set { if (_estimatedSize == value) return; _estimatedSize = value; OnPropertyChanged(); } }
    private long _estimatedSize;

    /// <summary>
    /// Link to the help documentation or website for the program, if available.
    /// </summary>
    public string? HelpLink { get => _helpLink; set { if (_helpLink == value) return; _helpLink = value; OnPropertyChanged(); } }
    private string? _helpLink;

    /// <summary>
    /// Telephone number for help or support related to the program, if available.
    /// </summary>
    public string? HelpTelephone { get => _helpTelephone; set { if (_helpTelephone == value) return; _helpTelephone = value; OnPropertyChanged(); } }
    private string? _helpTelephone;

    /// <summary>
    /// Date when the program was installed, if available.
    /// </summary>
    public string? InstallDate { get => _installDate; set { if (_installDate == value) return; _installDate = value; OnPropertyChanged(); } }
    private string? _installDate;

    /// <summary>
    /// Location where the program is installed.
    /// </summary>
    public string? InstallLocation { get => _installLocation; set { if (_installLocation == value) return; _installLocation = value; OnPropertyChanged(); } }
    private string? _installLocation;

    /// <summary>
    /// Source location from where the program was installed.
    /// </summary>
    public string? InstallSource { get => _installSource; set { if (_installSource == value) return; _installSource = value; OnPropertyChanged(); } }
    private string? _installSource;

    /// <summary>
    /// Language of the installed program.
    /// </summary>
    public string? Language { get => _language; set { if (_language == value) return; _language = value; OnPropertyChanged(); } }
    private string? _language;

    /// <summary>
    /// Path to modify the installation of the program.
    /// </summary>
    public string? ModifyPath { get => _modifyPath; set { if (_modifyPath == value) return; _modifyPath = value; OnPropertyChanged(); } }
    private string? _modifyPath;

    /// <summary>
    /// Indicates whether the program can be modified.
    /// </summary>
    public bool NoModify { get => _noModify; set { if (_noModify == value) return; _noModify = value; OnPropertyChanged(); } }
    private bool _noModify;

    /// <summary>
    /// Indicates whether the program can be removed or uninstalled.
    /// </summary>
    public bool NoRemove { get => _noRemove; set { if (_noRemove == value) return; _noRemove = value; OnPropertyChanged(); } }
    private bool _noRemove;

    /// <summary>
    /// Indicates whether the program supports repair functionality.
    /// </summary>
    public bool NoRepair { get => _noRepair; set { if (_noRepair == value) return; _noRepair = value; OnPropertyChanged(); } }
    private bool _noRepair;

    /// <summary>
    /// Publisher of the program.
    /// </summary>
    public string? Publisher { get => _publisher; set { if (_publisher == value) return; _publisher = value; OnPropertyChanged(); } }
    private string? _publisher;

    /// <summary>
    /// Link to the readme file of the program, if available.
    /// </summary>
    public string? Readme { get => _readme; set { if (_readme == value) return; _readme = value; OnPropertyChanged(); } }
    private string? _readme;

    /// <summary>
    /// Indicates whether the program is a system component.
    /// </summary>
    public bool SystemComponent { get => _systemComponent; set { if (_systemComponent == value) return; _systemComponent = value; OnPropertyChanged(); } }
    private bool _systemComponent;

    /// <summary>
    /// Command line string to uninstall the program quietly.
    /// </summary>
    public string? QuietUninstallString { get => _quietUninstallString; set { if (_quietUninstallString == value) return; _quietUninstallString = value; OnPropertyChanged(); } }
    private string? _quietUninstallString;

    /// <summary>
    /// Command line string used for uninstalling the program.
    /// </summary>
    public string? UninstallString { get => _uninstallString; set { if (_uninstallString == value) return; _uninstallString = value; OnPropertyChanged(); } }
    private string? _uninstallString;

    /// <summary>
    /// URL with information about the program.
    /// </summary>
    public string? UrlInfoAbout { get => _urlInfoAbout; set { if (_urlInfoAbout == value) return; _urlInfoAbout = value; OnPropertyChanged(); } }
    private string? _urlInfoAbout;

    /// <summary>
    /// URL for program updates information.
    /// </summary>
    public string? UrlUpdateInfo { get => _urlUpdateInfo; set { if (_urlUpdateInfo == value) return; _urlUpdateInfo = value; OnPropertyChanged(); } }
    private string? _urlUpdateInfo;

    /// <summary>
    /// Major version number of the program.
    /// </summary>
    public long VersionMajor { get => _versionMajor; set { if (_versionMajor == value) return; _versionMajor = value; OnPropertyChanged(); } }
    private long _versionMajor;

    /// <summary>
    /// Minor version number of the program.
    /// </summary>
    public long VersionMinor { get => _versionMinor; set { if (_versionMinor == value) return; _versionMinor = value; OnPropertyChanged(); } }
    private long _versionMinor;

    /// <summary>
    /// Indicates whether the program was installed using Windows Installer.
    /// </summary>
    public bool WindowsInstaller { get => _windowsInstaller; set { if (_windowsInstaller == value) return; _windowsInstaller = value; OnPropertyChanged(); } }
    private bool _windowsInstaller;

    /// <summary>
    /// Registry key associated with the program, if available.
    /// </summary>
    public string? RegKey { get => _regKey; set { if (_regKey == value) return; _regKey = value; OnPropertyChanged(); } }
    private string? _regKey;

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly IProgramInfoService _programInfoService;

    /// <summary>
    /// Initializes a new instance of the ProgramInfoData class.
    /// </summary>
    /// <param name="programInfoService">Service to handle program information logic.</param>
    public ProgramInfoData(IProgramInfoService programInfoService) => _programInfoService = programInfoService;

    /// <summary>
    /// Initializes a new instance of the ProgramInfoData class.
    /// </summary>
    public ProgramInfoData() => _programInfoService = new ProgramInfoService();

    /// <summary>
    /// Updates the current instance with information from another ProgramInfoData instance.
    /// </summary>
    /// <param name="programInfoData">The ProgramInfoData instance to update from.</param>
    public void UpdateFromDifferent(ProgramInfoData programInfoData) => _programInfoService.UpdateFromDifferent(this, programInfoData);

    /// <summary>
    /// Uninstalls the the program.
    /// </summary>
    /// <returns>A task that returns true if the uninstallation is successful.</returns>
    public async Task<bool> Uninstall() => await _programInfoService.Uninstall(this);

    /// <summary>
    /// Modifies the program.
    /// </summary>
    /// <returns>A task that returns true if the modification is successful.</returns>
    public async Task<bool> Modify() => await _programInfoService.Modify(this);

    /// <summary>
    /// Opens the registry entry associated with the program.
    /// </summary>
    /// <returns>A task that returns true if the registry is successfully opened.</returns>
    public async Task<bool> OpenRegistry() => await _programInfoService.OpenRegistry(this);

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not ProgramInfoData programInfoData)
            return false;

        foreach (PropertyInfo property in GetType().GetProperties())
        {
            if (property.Name == nameof(DisplayIcon))
                continue;
            if (!Equals(property.GetValue(this), property.GetValue(programInfoData)))
                return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;

            foreach (PropertyInfo property in GetType().GetProperties())
            {
                if (property.Name == nameof(DisplayIcon))
                    continue;

                var value = property.GetValue(this);
                hash = (hash * 23) + (value != null ? value.GetHashCode() : 0);
            }

            return hash;
        }
    }

    public override string? ToString() => DisplayName;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
