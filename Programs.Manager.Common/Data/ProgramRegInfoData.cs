using Programs.Manager.Common.Service.ProgramRegInfo;
using System.Drawing;

namespace Programs.Manager.Common.Data;

/// <summary>
/// Contains information from the registry about a program.
/// </summary>
public sealed partial class ProgramRegInfoData
{
    /// <summary>
    /// Gets or sets the unique identifier for the program.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the authorized CDF (Channel Definition Format) prefix.
    /// </summary>
    public string AuthorizedCDFPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets comments associated with the program.
    /// </summary>
    public string Comments { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact information for support or further information.
    /// </summary>
    public string Contact { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file path or URL to the program's display icon.
    /// </summary>
    public string DisplayIcon { get => _displayIconPath; set { _displayIconPath = value; } }

    /// <summary>
    /// Gets or sets the file path or URL to the program's display icon.
    /// </summary>
    public string DisplayIconPath { get; set; }
    private string _displayIconPath = string.Empty;

    /// <summary>
    /// Gets or sets the display name of the program.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display version string of the program.
    /// </summary>
    public string DisplayVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the estimated size of the program, typically in KB.
    /// </summary>
    public string EstimatedSize { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL to the help documentation or website.
    /// </summary>
    public string HelpLink { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the help telephone number.
    /// </summary>
    public string HelpTelephone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the installation date of the program.
    /// </summary>
    public string InstallDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the directory path where the program was installed.
    /// </summary>
    public string InstallDir { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the installation location of the program.
    /// </summary>
    public string InstallLocation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source path used during the program's installation.
    /// </summary>
    public string InstallSource { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the language code of the program.
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the major version number of the program.
    /// </summary>
    public string MajorVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the minor version number of the program.
    /// </summary>
    public string MinorVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path for modifying the program's installation.
    /// </summary>
    public string ModifyPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the program can be modified.
    /// </summary>
    public string NoModify { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the program can be removed or uninstalled.
    /// </summary>
    public string NoRemove { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the program supports repair functionality.
    /// </summary>
    public string NoRepair { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the publisher of the program.
    /// </summary>
    public string Publisher { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the location of the program's readme file.
    /// </summary>
    public string Readme { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the size of the program. This could be the disk space it uses.
    /// </summary>
    public string Size { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the program is a system component.
    /// </summary>
    public string SystemComponent { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the command line string used for a quiet uninstallation.
    /// </summary>
    public string QuietUninstallString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the command line string used to uninstall the program.
    /// </summary>
    public string UninstallString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL for information about the program.
    /// </summary>
    public string UrlInfoAbout { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL for updates related to the program.
    /// </summary>
    public string UrlUpdateInfo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version of the program.
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the major part of the version number.
    /// </summary>
    public string VersionMajor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the minor part of the version number.
    /// </summary>
    public string VersionMinor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the program was installed by the Windows Installer.
    /// </summary>
    public string WindowsInstaller { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the registry key associated with the program.
    /// </summary>
    public string RegKey { get; set; } = string.Empty;

    private readonly IProgramRegInfoService _programRegInfoService;

    /// <summary>
    /// Initializes a new instance of the ProgramRegInfoData class.
    /// </summary>
    /// <param name="programRegInfoService">Service to handle program registry information logic.</param>
    public ProgramRegInfoData(IProgramRegInfoService programRegInfoService) => _programRegInfoService = programRegInfoService;

    /// <summary>
    /// Gets the icon associated with the program.
    /// </summary>
    /// <returns>A Bitmap that represents the Icon.</returns>
    public Bitmap? GetIcon() => _programRegInfoService.GetIcon(this);

    public override string ToString() => DisplayName;
}
