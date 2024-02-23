namespace Programs.Manager.Reader.Win.Data;

/// <summary>
/// Specifies the locations where information about installed programs can be found in the registry.
/// </summary>
public enum ProgramInfoLocation
{
    LocalMachineUninstallLocation64,
    LocalMachineUninstallLocation32,
    CurrentUserUninstallLocation64,
    CurrentUserUninstallLocation32,
}
