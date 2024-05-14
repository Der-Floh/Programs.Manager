using Microsoft.Win32;

namespace WindowsRegistry.Serializer.Services;
public class RegistryService
{
    private readonly Dictionary<string, Func<string, bool, RegistryKey?>> registryMap = new()
    {
        {"HKLM", Registry.LocalMachine.OpenSubKey},
        {"HKEY_LOCAL_MACHINE", Registry.LocalMachine.OpenSubKey},

        {"HKCU", Registry.CurrentUser.OpenSubKey},
        {"HKEY_CURRENT_USER", Registry.CurrentUser.OpenSubKey},

        {"HKU", Registry.Users.OpenSubKey},
        {"HKEY_USERS", Registry.Users.OpenSubKey},

        {"HKCC", Registry.CurrentConfig.OpenSubKey},
        {"HKEY_CURRENT_CONFIG", Registry.CurrentConfig.OpenSubKey},

        {"HKCR", Registry.ClassesRoot.OpenSubKey},
        {"HKEY_CLASSES_ROOT", Registry.ClassesRoot.OpenSubKey},

        {"HKPD", Registry.PerformanceData.OpenSubKey},
        {"HKEY_PERFORMANCE_DATA", Registry.PerformanceData.OpenSubKey},
    };

    private RegistryKey? GetRegistryKey(string path)
    {
        path = path.Replace("Computer" + Path.DirectorySeparatorChar, "", StringComparison.CurrentCultureIgnoreCase);
        var paths = path.Split(Path.DirectorySeparatorChar, 2);
        var location = paths[0].ToUpper();
        path = paths[1];
        if (!registryMap.TryGetValue(location, out var openSubKeyFunc))
            return null;

        return openSubKeyFunc(path, false);
    }

    public object? Get(in string path, in string key)
    {
        var regKey = GetRegistryKey(path);
        if (regKey is null)
            return null;

        return regKey.GetValue(key);
    }

    public object? Get(in string path)
    {
        var keyName = Path.GetFileName(path);
        var keyPath = Path.GetDirectoryName(path);

        if (keyPath is null)
            return null;

        var regKey = GetRegistryKey(keyPath);
        if (regKey is null)
            return null;
       
        return regKey.GetValue(keyName);
    }

    public Dictionary<string, string?>? GetRegKeyValues(in string path)
    {
        var keys = new Dictionary<string, string?>();
        var regKey = GetRegistryKey(path);
        if (regKey is null)
            return keys;

        foreach (var valueName in regKey.GetValueNames())
            keys.Add(valueName, regKey.GetValue(valueName)?.ToString());

        return keys;
    }

    public string[]? GetSubKeyNames(in string path)
    {
        var regKey = GetRegistryKey(path);
        return regKey?.GetSubKeyNames();
    }
}
