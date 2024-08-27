using System.ComponentModel;
using System.Globalization;

namespace ProgramInfos.Manager.Abstractions.Data;

/// <summary>
/// Contains information about a program.
/// </summary>
public interface IProgramInfoData : INotifyPropertyChanged
{
    /// <summary>
    /// Source key to identify the implementation of a specific plugin.
    /// </summary>
    public string SourceKey { get; }

    /// <summary>
    /// Unique identifier for the program.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Authorized CDF (Channel Definition Format) prefix, if available.
    /// </summary>
    public string? AuthorizedCDFPrefix { get; set; }

    /// <summary>
    /// Comments associated with the program, if available.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Contact information for the program's publisher or support, if available.
    /// </summary>
    public string? Contact { get; set; }

    /// <summary>
    /// MemoryStream representing the icon of the program, if available.
    /// </summary>
    public MemoryStream? DisplayIconStream { get; set; }

    /// <summary>
    /// Path to the icon representing the program, if available.
    /// </summary>
    public string? DisplayIconPath { get; set; }

    /// <summary>
    /// Index of the icon representing the program, if available.
    /// </summary>
    public int DisplayIconIndex { get; set; }

    /// <summary>
    /// Group name of the icon representing the program, if available.
    /// </summary>
    public string? DisplayIconGroupName { get; set; }

    /// <summary>
    /// Display name of the program as registered in the system, if available.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Version of the program as displayed in the registry, if available.
    /// </summary>
    public string? DisplayVersion { get; set; }

    /// <summary>
    /// Estimated size of the program on disk, in bytes, if available.
    /// </summary>
    public long EstimatedSize { get; set; }

    /// <summary>
    /// Link to the help documentation or website for the program, if available.
    /// </summary>
    public string? HelpLink { get; set; }

    /// <summary>
    /// Telephone number for help or support related to the program, if available.
    /// </summary>
    public string? HelpTelephone { get; set; }

    /// <summary>
    /// Date when the program was installed, if available.
    /// </summary>
    public DateTime? InstallDate { get; set; }

    /// <summary>
    /// Location where the program is installed, if available.
    /// </summary>
    public string? InstallLocation { get; set; }

    /// <summary>
    /// Source location from where the program was installed, if available.
    /// </summary>
    public string? InstallSource { get; set; }

    /// <summary>
    /// Culture of the installed program, if available.
    /// </summary>
    public CultureInfo? CultureInfo { get; set; }

    /// <summary>
    /// Path to modify the installation of the program, if available.
    /// </summary>
    public string? ModifyPath { get; set; }

    /// <summary>
    /// Indicates whether the program can be modified.
    /// </summary>
    public bool NoModify { get; set; }

    /// <summary>
    /// Indicates whether the program can be removed or uninstalled.
    /// </summary>
    public bool NoRemove { get; set; }

    /// <summary>
    /// Indicates whether the program supports repair functionality.
    /// </summary>
    public bool NoRepair { get; set; }

    /// <summary>
    /// Publisher of the program, if available.
    /// </summary>
    public string? Publisher { get; set; }

    /// <summary>
    /// Link to the readme file of the program, if available.
    /// </summary>
    public string? Readme { get; set; }

    /// <summary>
    /// Indicates whether the program is a system component.
    /// </summary>
    public bool SystemComponent { get; set; }

    /// <summary>
    /// Command line string to uninstall the program quietly, if available.
    /// </summary>
    public string? QuietUninstallString { get; set; }

    /// <summary>
    /// Command line string used for uninstalling the program, if available.
    /// </summary>
    public string? UninstallString { get; set; }

    /// <summary>
    /// URL with information about the program, if available.
    /// </summary>
    public string? UrlInfoAbout { get; set; }

    /// <summary>
    /// URL for program updates information, if available.
    /// </summary>
    public string? UrlUpdateInfo { get; set; }

    /// <summary>
    /// Major version number of the program (-1 if not available).
    /// </summary>
    public int VersionMajor { get; set; }

    /// <summary>
    /// Minor version number of the program (-1 if not available).
    /// </summary>
    public int VersionMinor { get; set; }

    /// <summary>
    /// Indicates whether the program was installed using Windows Installer.
    /// </summary>
    public bool WindowsInstaller { get; set; }

    /// <summary>
    /// Registry key associated with the program.
    /// </summary>
    public string RegKey { get; set; }
}
