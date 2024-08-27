using Microsoft.Win32;

namespace ProgramInfos.Manager.Reg.Service.CRegistry;

/// <summary>
/// Service for managing operations related to the Windows registry.
/// </summary>
public interface IRegistryService
{
    /// <summary>
    /// Checks if the application is running as an administrator.
    /// </summary>
    /// <returns>True if the application is running as an administrator.</returns>
    bool IsRunningAsAdmin();

    /// <summary>
    /// Opens the registry editor at the specified registry key.
    /// </summary>
    /// <param name="regKey">The registry key to open in the registry editor.</param>
    /// <returns>True if the operation succeeds, otherwise false.</returns>
    bool OpenAt(string regKey, bool startAdminProcess = true);

    /// <summary>
    /// Opens the registry editor.
    /// </summary>
    /// <returns>True if the operation succeeds, otherwise false.</returns>
    bool Open();

    /// <summary>
    /// Retrieves the values associated with the specified registry key.
    /// </summary>
    /// <param name="path">The path of the registry key.</param>
    /// <returns>A dictionary containing the value names and their corresponding values.</returns>
    Dictionary<string, object>? GetRegKeyValues(string path);

    /// <summary>
    /// Get all subkeys from a registry key.
    /// </summary>
    /// <param name="path">The registry key path.</param>
    /// <returns> Array of subkeys.</returns>
    string[]? GetSubKeyNames(string path);

    /// <summary>
    /// Tries to set a registry value to the given value.
    /// </summary>
    /// <param name="path">The registry key path for which to set the value.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="startAdminProcess">Whether to start a Admin process if App is not running as Admin already.</param>
    /// <param name="kind">The type of value to set. Default is String.</param>
    /// <returns>True if the operation was successful.</returns>
    bool TrySet(string path, object value, bool startAdminProcess, RegistryValueKind kind = RegistryValueKind.String);

    /// <summary>
    /// Set a registry value to the given value. Throws an error if failed
    /// </summary>
    /// <param name="path">The registry key path for which to set the value.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="startAdminProcess">Whether to start a Admin process if App is not running as Admin already.</param>
    /// <param name="kind">The type of value to set. Default is String.</param>
    void Set(string path, object value, bool startAdminProcess, RegistryValueKind kind = RegistryValueKind.String);

    /// <summary>
    /// Tries to set a registry value to the given value using a command line process.
    /// </summary>
    /// <param name="path">The registry key path for which to set the value.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="kind">The type of value to set. Default is String.</param>
    /// <returns>True if the operation was successful.</returns>
    bool TrySetKeyWithProcess(string path, object value, RegistryValueKind kind = RegistryValueKind.String);
}
