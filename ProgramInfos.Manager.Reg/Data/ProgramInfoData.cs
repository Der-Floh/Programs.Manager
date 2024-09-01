using ProgramInfos.Manager.Abstractions.Data;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using WindowsRegistry.Serializer;
using WindowsRegistry.Serializer.Attributes;

namespace ProgramInfos.Manager.Reg.Data;

/// <inheritdoc cref="IProgramInfoData"/>
public class ProgramInfoData : IProgramInfoData
{
    /// <summary>
    /// Name of the source key to identify this plugin.
    /// </summary>
    public const string SourceKeyName = "Windows Registry";

    /// <inheritdoc/>
    [RegistryIgnore]
    public string SourceKey => SourceKeyName;

    /// <inheritdoc/>
    public string Id { get => _id; set => SetProperty(ref _id, value); }
    private string _id = string.Empty;

    /// <inheritdoc/>
    public string? AuthorizedCDFPrefix { get => _authorizedCDFPrefix; set => SetProperty(ref _authorizedCDFPrefix, value); }
    private string? _authorizedCDFPrefix;

    /// <inheritdoc/>
    public string? Comments { get => _comments; set => SetProperty(ref _comments, value); }
    private string? _comments;

    /// <inheritdoc/>
    public string? Contact { get => _contact; set => SetProperty(ref _contact, value); }
    private string? _contact;

    /// <inheritdoc/>
    [RegistryIgnore]
    public MemoryStream? DisplayIconStream { get => _displayIconStream; set => SetProperty(ref _displayIconStream, value); }
    private MemoryStream? _displayIconStream;

    /// <inheritdoc/>
    [RegistryIgnore]
    public IIconInfo? DisplayIconInfo { get => _displayIconInfo; set => SetProperty(ref _displayIconInfo, value); }
    private IIconInfo? _displayIconInfo;

    /// <inheritdoc/>
    public string? DisplayIcon { get => _displayIcon; set => SetProperty(ref _displayIcon, value); }
    private string? _displayIcon;

    /// <inheritdoc/>
    public string? DisplayName { get => _displayName; set => SetProperty(ref _displayName, value); }
    private string? _displayName;

    /// <inheritdoc/>
    public string? DisplayVersion { get => _displayVersion; set => SetProperty(ref _displayVersion, value); }
    private string? _displayVersion;

    /// <inheritdoc/>
    [RegistryDeserializeNames("EstimatedSize", "Size")]
    [RegistryDeserializerPostProcess(typeof(EstimatedSizePostProcess))]
    public long EstimatedSize { get => _estimatedSize; set => SetProperty(ref _estimatedSize, value); }
    private long _estimatedSize = -1;

    /// <inheritdoc/>
    public string? HelpLink { get => _helpLink; set => SetProperty(ref _helpLink, value); }
    private string? _helpLink;

    /// <inheritdoc/>
    public string? HelpTelephone { get => _helpTelephone; set => SetProperty(ref _helpTelephone, value); }
    private string? _helpTelephone;

    /// <inheritdoc/>
    public DateTime? InstallDate { get => _installDate; set => SetProperty(ref _installDate, value); }
    private DateTime? _installDate;

    /// <inheritdoc/>
    [RegistryDeserializeNames("InstallLocation", "InstallDir")]
    public string? InstallLocation { get => _installLocation; set => SetProperty(ref _installLocation, value); }
    private string? _installLocation;

    /// <inheritdoc/>
    public string? InstallSource { get => _installSource; set => SetProperty(ref _installSource, value); }
    private string? _installSource;

    /// <inheritdoc/>
    [RegistryIgnore]
    public CultureInfo? CultureInfo { get => _cultureInfo; set => SetProperty(ref _cultureInfo, value); }
    private CultureInfo? _cultureInfo;

    /// <summary>
    /// Language of the installed program, if available.
    /// </summary>
    public string? Language { get => _language; set => SetProperty(ref _language, value); }
    private string? _language;

    /// <inheritdoc/>
    public string? ModifyPath { get => _modifyPath; set => SetProperty(ref _modifyPath, value); }
    private string? _modifyPath;

    /// <inheritdoc/>
    public bool NoModify { get => _noModify; set => SetProperty(ref _noModify, value); }
    private bool _noModify;

    /// <inheritdoc/>
    public bool NoRemove { get => _noRemove; set => SetProperty(ref _noRemove, value); }
    private bool _noRemove;

    /// <inheritdoc/>
    public bool NoRepair { get => _noRepair; set => SetProperty(ref _noRepair, value); }
    private bool _noRepair;

    /// <inheritdoc/>
    public string? Publisher { get => _publisher; set => SetProperty(ref _publisher, value); }
    private string? _publisher;

    /// <inheritdoc/>
    public string? Readme { get => _readme; set => SetProperty(ref _readme, value); }
    private string? _readme;

    /// <inheritdoc/>
    public bool SystemComponent { get => _systemComponent; set => SetProperty(ref _systemComponent, value); }
    private bool _systemComponent;

    /// <inheritdoc/>
    public string? QuietUninstallString { get => _quietUninstallString; set => SetProperty(ref _quietUninstallString, value); }
    private string? _quietUninstallString;

    /// <inheritdoc/>
    public string? UninstallString { get => _uninstallString; set => SetProperty(ref _uninstallString, value); }
    private string? _uninstallString;

    /// <inheritdoc/>
    public string? UrlInfoAbout { get => _urlInfoAbout; set => SetProperty(ref _urlInfoAbout, value); }
    private string? _urlInfoAbout;

    /// <inheritdoc/>
    public string? UrlUpdateInfo { get => _urlUpdateInfo; set => SetProperty(ref _urlUpdateInfo, value); }
    private string? _urlUpdateInfo;

    /// <inheritdoc/>
    [RegistryDeserializeNames("VersionMajor", "MajorVersion")]
    public int VersionMajor { get => _versionMajor; set => SetProperty(ref _versionMajor, value); }
    private int _versionMajor = -1;

    /// <inheritdoc/>
    [RegistryDeserializeNames("VersionMinor", "MinorVersion")]
    public int VersionMinor { get => _versionMinor; set => SetProperty(ref _versionMinor, value); }
    private int _versionMinor = -1;

    /// <inheritdoc/>
    public bool WindowsInstaller { get => _windowsInstaller; set => SetProperty(ref _windowsInstaller, value); }
    private bool _windowsInstaller;

    /// <inheritdoc/>
    public string RegKey { get => _regKey; set => SetProperty(ref _regKey, value); }
    private string _regKey = string.Empty;

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not ProgramInfoData programInfoData)
            return false;

        foreach (var property in GetType().GetProperties())
        {
            if (property.Name == nameof(DisplayIconStream))
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
                if (property.Name == nameof(DisplayIconStream))
                    continue;

                var value = property.GetValue(this);
                hash = (hash * 23) + (value != null ? value.GetHashCode() : 0);
            }

            return hash;
        }
    }

    public override string? ToString() => DisplayName + " - " + RegKey;

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

/// <summary>
/// Post process for converting the estimated size from bytes to KB/>
/// </summary>
public class EstimatedSizePostProcess : RegistryDeserializerPostProcess<long>
{
    public override long Effect(long data)
    {
        var intVal = (uint)data;
        return (long)(intVal * 1000);
    }
}
