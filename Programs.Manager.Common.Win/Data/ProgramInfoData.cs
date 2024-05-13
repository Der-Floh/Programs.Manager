using Programs.Manager.Common.Win.Service;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using WindowsRegistry.Serializer;
using WindowsRegistry.Serializer.Attributes;

namespace Programs.Manager.Common.Win.Data;

/// <summary>
/// Contains information about a program.
/// </summary>
public class ProgramInfoData : INotifyPropertyChanged
{
    /// <summary>
    /// Unique identifier for the program. Is equivalent to the registry Program Guid
    /// </summary>
    public string Id { get => _id; set => SetProperty(ref _id, value); }
    private string _id = string.Empty;

    /// <summary>
    /// Gets or sets the authorized CDF (Channel Definition Format) prefix.
    /// </summary>
    public string? AuthorizedCDFPrefix { get => _authorizedCDFPrefix; set => SetProperty(ref _authorizedCDFPrefix, value); }
    private string? _authorizedCDFPrefix;

    /// <summary>
    /// Comments associated with the program, if available.
    /// </summary>
    public string? Comments { get => _comments; set => SetProperty(ref _comments, value); }
    private string? _comments;

    /// <summary>
    /// Contact information for the program's publisher or support.
    /// </summary>
    public string? Contact { get => _contact; set => SetProperty(ref _contact, value); }
    private string? _contact;

    /// <summary>
    /// Icon representing the program, if available.
    /// </summary>
    [RegistryIgnore]
    public Bitmap? DisplayIconImage { get => _displayIconImage; set => SetProperty(ref _displayIconImage, value); }
    private Bitmap? _displayIconImage;

    /// <summary>
    /// Path to the icon representing the program, if available.
    /// </summary>
    public string? DisplayIcon { get => _displayIcon; set => SetProperty(ref _displayIcon, value); }
    private string? _displayIcon;

    /// <summary>
    /// Display name of the program as registered in the system.
    /// </summary>
    public string? DisplayName { get => _displayName; set => SetProperty(ref _displayName, value); }
    private string? _displayName;

    /// <summary>
    /// Version of the program as displayed in the registry.
    /// </summary>
    public string? DisplayVersion { get => _displayVersion; set => SetProperty(ref _displayVersion, value); }
    private string? _displayVersion;

    /// <summary>
    /// Estimated size of the program on disk, in bytes.
    /// </summary>
    [RegistryNames("EstimatedSize", "Size")]
    [RegistryDeserializerPostProcess(typeof(EstimatedSizePostProcess))]
    public long EstimatedSize { get => _estimatedSize; set => SetProperty(ref _estimatedSize, value); }
    private long _estimatedSize = -1;

    /// <summary>
    /// Link to the help documentation or website for the program, if available.
    /// </summary>
    public string? HelpLink { get => _helpLink; set => SetProperty(ref _helpLink, value); }
    private string? _helpLink;

    /// <summary>
    /// Telephone number for help or support related to the program, if available.
    /// </summary>
    public string? HelpTelephone { get => _helpTelephone; set => SetProperty(ref _helpTelephone, value); }
    private string? _helpTelephone;

    /// <summary>
    /// Date when the program was installed, if available.
    /// </summary>
    public DateTime? InstallDate { get => _installDate; set => SetProperty(ref _installDate, value); }
    private DateTime? _installDate;

    /// <summary>
    /// Location where the program is installed.
    /// </summary>
    [RegistryNames("InstallLocation", "InstallDir")]
    public string? InstallLocation { get => _installLocation; set => SetProperty(ref _installLocation, value); }
    private string? _installLocation;

    /// <summary>
    /// Source location from where the program was installed.
    /// </summary>
    public string? InstallSource { get => _installSource; set => SetProperty(ref _installSource, value); }
    private string? _installSource;

    /// <summary>
    /// Language of the installed program.
    /// </summary>
    public string? Language { get => _language; set => SetProperty(ref _language, value); }
    private string? _language;

    /// <summary>
    /// Path to modify the installation of the program.
    /// </summary>
    public string? ModifyPath { get => _modifyPath; set => SetProperty(ref _modifyPath, value); }
    private string? _modifyPath;

    /// <summary>
    /// Indicates whether the program can be modified.
    /// </summary>
    public bool NoModify { get => _noModify; set => SetProperty(ref _noModify, value); }
    private bool _noModify;

    /// <summary>
    /// Indicates whether the program can be removed or uninstalled.
    /// </summary>
    public bool NoRemove { get => _noRemove; set => SetProperty(ref _noRemove, value); }
    private bool _noRemove;

    /// <summary>
    /// Indicates whether the program supports repair functionality.
    /// </summary>
    public bool NoRepair { get => _noRepair; set => SetProperty(ref _noRepair, value); }
    private bool _noRepair;

    /// <summary>
    /// Publisher of the program.
    /// </summary>
    public string? Publisher { get => _publisher; set => SetProperty(ref _publisher, value); }
    private string? _publisher;

    /// <summary>
    /// Link to the readme file of the program, if available.
    /// </summary>
    public string? Readme { get => _readme; set => SetProperty(ref _readme, value); }
    private string? _readme;

    /// <summary>
    /// Indicates whether the program is a system component.
    /// </summary>
    public bool SystemComponent { get => _systemComponent; set => SetProperty(ref _systemComponent, value); }
    private bool _systemComponent;

    /// <summary>
    /// Command line string to uninstall the program quietly.
    /// </summary>
    public string? QuietUninstallString { get => _quietUninstallString; set => SetProperty(ref _quietUninstallString, value); }
    private string? _quietUninstallString;

    /// <summary>
    /// Command line string used for uninstalling the program.
    /// </summary>
    public string? UninstallString { get => _uninstallString; set => SetProperty(ref _uninstallString, value); }
    private string? _uninstallString;

    /// <summary>
    /// URL with information about the program.
    /// </summary>
    public string? UrlInfoAbout { get => _urlInfoAbout; set => SetProperty(ref _urlInfoAbout, value); }
    private string? _urlInfoAbout;

    /// <summary>
    /// URL for program updates information.
    /// </summary>
    public string? UrlUpdateInfo { get => _urlUpdateInfo; set => SetProperty(ref _urlUpdateInfo, value); }
    private string? _urlUpdateInfo;

    /// <summary>
    /// Major version number of the program.
    /// </summary>
    [RegistryNames("VersionMajor", "MajorVersion")]
    public int VersionMajor { get => _versionMajor; set => SetProperty(ref _versionMajor, value); }
    private int _versionMajor = -1;

    /// <summary>
    /// Minor version number of the program.
    /// </summary>
    [RegistryNames("VersionMinor", "MinorVersion")]
    public int VersionMinor { get => _versionMinor; set => SetProperty(ref _versionMinor, value); }
    private int _versionMinor = -1;

    /// <summary>
    /// Indicates whether the program was installed using Windows Installer.
    /// </summary>
    public bool WindowsInstaller { get => _windowsInstaller; set => SetProperty(ref _windowsInstaller, value); }
    private bool _windowsInstaller;

    /// <summary>
    /// Registry key associated with the program, if available.
    /// </summary>
    public string? RegKey { get => _regKey; set => SetProperty(ref _regKey, value); }
    private string? _regKey;

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Updates the current instance with information from another ProgramInfoData instance.
    /// </summary>
    /// <param name="programInfoData">The ProgramInfoData instance to update from.</param>
    public void UpdateFromDifferent(IProgramInfoService programInfoService, ProgramInfoData programInfoData) => programInfoService.UpdateFromDifferent(this, programInfoData);

    /// <summary>
    /// Uninstalls the the program.
    /// </summary>
    /// <returns>A task that returns true if the uninstallation is successful.</returns>
    public async Task<bool> Uninstall(IProgramInfoService programInfoService) => await programInfoService.Uninstall(this);

    /// <summary>
    /// Modifies the program.
    /// </summary>
    /// <returns>A task that returns true if the modification is successful.</returns>
    public async Task<bool> Modify(IProgramInfoService programInfoService) => await programInfoService.Modify(this);

    /// <summary>
    /// Opens the registry entry associated with the program.
    /// </summary>
    /// <returns>A task that returns true if the registry is successfully opened.</returns>
    public bool OpenRegistry(IProgramInfoService programInfoService) => programInfoService.OpenRegistry(this);

    /// <summary>
    /// Fetches fallback values for the program.
    /// </summary>
    public void FetchFallbackProperties(IProgramInfoService programInfoService) => programInfoService.FetchFallbackProperties(this);

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not ProgramInfoData programInfoData)
            return false;

        foreach (var property in GetType().GetProperties())
        {
            if (property.Name == nameof(DisplayIconImage))
                continue;
            if (!Equals(property.GetValue(this), property.GetValue(programInfoData)))
                return false;
        }
        return true;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;

            foreach (var property in GetType().GetProperties())
            {
                if (property.Name == nameof(DisplayIconImage))
                    continue;

                var value = property.GetValue(this);
                hash = (hash * 23) + (value != null ? value.GetHashCode() : 0);
            }

            return hash;
        }
    }

    public override string? ToString() => DisplayName;

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public class EstimatedSizePostProcess : RegistryDeserializerPostProcess<long>
{
    public override long Effect(long data)
    {
        var intVal = (uint)data;
        return (long)(intVal * 1000);
    }
}
