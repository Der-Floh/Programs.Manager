using Microsoft.Win32;
using System.Security;
using System.Security.Principal;

namespace Programs.Manager.Reader.Win.Utility;

/// <summary>
/// Provides methods for accessing and manipulating Windows Registry entries.
/// </summary>
public static class RegistryHelper
{
    private static RegistryKey? OpenPath(string path, bool writable = false)
    {
        var paths = path.Split('\\', 2);
        var location = paths[0];
        path = paths[1];

        switch (location)
        {
            case "HKEY_CLASSES_ROOT":
                return Registry.ClassesRoot.OpenSubKey(path, writable);
            case "HKEY_CURRENT_USER":
                return Registry.CurrentUser.OpenSubKey(path, writable);
            case "HKEY_LOCAL_MACHINE":
                return Registry.LocalMachine.OpenSubKey(path, writable);
            case "HKEY_USERS":
                return Registry.Users.OpenSubKey(path, writable);
            default:
                break;
        }
        return null;
    }

    /// <summary>
    /// Retrieves the value of a specified registry key.
    /// </summary>
    /// <param name="path">The path of the registry key.</param>
    /// <param name="key">The name of the value within the registry key to retrieve.</param>
    /// <returns>The value of the registry key, or null if the key does not exist.</returns>
    public static object? GetRegKeyValue(string path, string key)
    {
        RegistryKey? regKey = OpenPath(path);
        return regKey?.GetValue(key);
    }

    /// <summary>
    /// Retrieves the value of a specified registry key.
    /// </summary>
    /// <param name="path">The path of the registry key including the key.</param>
    /// <returns>The value of the registry key, or null if the key does not exist.</returns>
    public static object? GetRegKeyValue(string path)
    {
        var key = Path.GetFileName(path);
        path = Path.GetDirectoryName(path)!;

        RegistryKey? regKey = OpenPath(path);
        return regKey?.GetValue(key);
    }

    /// <summary>
    /// Retrieves all values from a specified registry key.
    /// </summary>
    /// <param name="path">The path of the registry key.</param>
    /// <returns>A dictionary containing all values within the specified registry key.</returns>
    public static Dictionary<string, object?> GetRegKeyValues(string path)
    {
        var keys = new Dictionary<string, object?>();
        RegistryKey? regKey = OpenPath(path);
        foreach (var valueName in regKey!.GetValueNames())
        {
            keys.Add(valueName, GetRegKeyValue(path, valueName));
        }
        return keys;
    }

    /// <summary>
    /// Retrieves all subkey names under a specified registry key.
    /// </summary>
    /// <param name="path">The path of the registry key.</param>
    /// <returns>An array of subkey names under the specified registry key.</returns>
    public static string[] GetRegKeys(string path)
    {
        var keys = new List<string>();
        RegistryKey? regKey = OpenPath(path);
        if (regKey is null)
            return keys.ToArray();
        foreach (var subKey in regKey.GetSubKeyNames())
        {
            keys.Add(subKey);
        }
        return keys.ToArray();
    }

    /// <summary>
    /// Sets the value of a specified registry key.
    /// </summary>
    /// <param name="path">The path of the registry key.</param>
    /// <param name="key">The name of the value within the registry key to set.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="kind">The type of the value.</param>
    /// <returns>True if the operation is successful.</returns>
    /// <exception cref="SecurityException">Thrown if the application does not have administrator privileges.</exception>
    public static bool SetKey(string path, string key, object value, RegistryValueKind kind = RegistryValueKind.None)
    {
        if (!IsRunningAsAdmin())
            return false;

        RegistryKey? regKey = OpenPath(path, true);
        if (kind == RegistryValueKind.None)
            regKey?.SetValue(key, value);
        else
            regKey?.SetValue(key, value, kind);
        return true;
    }

    /// <summary>
    /// Sets the value of a specified registry key.
    /// </summary>
    /// <param name="path">The full path of the registry key, including the key name.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="kind">The type of the value.</param>
    /// <returns>True if the operation is successful.</returns>
    /// <exception cref="SecurityException">Thrown if the application does not have administrator privileges.</exception>
    public static bool SetKey(string path, object value, RegistryValueKind kind = RegistryValueKind.None)
    {
        if (!IsRunningAsAdmin())
            return false;

        var key = Path.GetFileName(path);
        path = Path.GetDirectoryName(path)!;

        RegistryKey? regKey = OpenPath(path, true);
        if (kind == RegistryValueKind.None)
            regKey?.SetValue(key, value);
        else
            regKey?.SetValue(key, value, kind);
        return true;
    }

    /// <summary>
    /// Checks if the current process is running with administrator privileges.
    /// </summary>
    /// <returns>True if the current process is running as an administrator, otherwise false.</returns>
    public static bool IsRunningAsAdmin()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}
