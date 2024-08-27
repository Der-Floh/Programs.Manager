using Microsoft.Win32;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Principal;

namespace ProgramInfos.Manager.Reg.Service.CRegistry;

/// <inheritdoc cref="IRegistryService"/>
public sealed class RegistryService : IRegistryService
{
    private const string RunAsVerb = "runas";
    private const string LastKey = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit\LastKey";

    private static readonly ConcurrentDictionary<Type, Delegate> _conversionCache = new ConcurrentDictionary<Type, Delegate>();

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

    private readonly Dictionary<RegistryValueKind, string> registryValueTypesMap = new()
    {
        { RegistryValueKind.String, "REG_SZ" },
        { RegistryValueKind.DWord, "REG_DWORD" },
        { RegistryValueKind.QWord, "REG_QWORD" },
        { RegistryValueKind.MultiString, "REG_MULTI_SZ" },
        { RegistryValueKind.ExpandString, "REG_EXPAND_SZ" },
        { RegistryValueKind.Binary, "REG_BINARY" }
    };

    /// <summary>
    /// Gets the <see cref="RegistryKey"/> for the given path.
    /// </summary>
    /// <param name="path">The path for which to get the key.</param>
    /// <param name="writable">Whether or not the key should be writable.</param>
    /// <returns>The <see cref="RegistryKey"/> for the given path or null if the path was not found.</returns>
    private RegistryKey? GetRegistryKey(string path, bool writable = false)
    {
        path = path.Replace("Computer" + Path.DirectorySeparatorChar, "", StringComparison.CurrentCultureIgnoreCase);
        var paths = path.Split(Path.DirectorySeparatorChar, 2);
        var location = paths[0].ToUpper();
        path = paths[1];

        return !registryMap.TryGetValue(location, out var openSubKeyFunc) ? null : openSubKeyFunc(paths[1], writable);
    }

    /// <summary>
    /// Retrieves the value associated with the specified registry key.
    /// </summary>
    /// <param name="path">The path of the registry key.</param>
    /// <param name="key">The name of the value to retrieve.</param>
    /// <returns>The value associated with the specified registry key, or null if the key or value does not exist.</returns>
    public object? Get(string path, string key)
    {
        var regKey = GetRegistryKey(path);
        return regKey?.GetValue(key);
    }

    /// <summary>
    /// Retrieves the value associated with the specified registry key.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="path">The path of the registry key.</param>
    /// <param name="key">The name of the registry key.</param>
    /// <returns>The value associated with the specified registry key converted to the specified type, or null if the key does not exist.</returns>
    public T? Get<T>(string path, string key)
    {
        var value = Get(path, key);
        return value is null ? default : ConvertValue<T>(value);
    }

    /// <summary>
    /// Retrieves the value associated with the specified registry key.
    /// </summary>
    /// <param name="path">The path of the registry key.</param>
    /// <returns>The value object associated with the specified registry key, or null if the key does not exist.</returns>
    public object? Get(string path)
    {
        var keyName = Path.GetFileName(path);
        var keyPath = Path.GetDirectoryName(path);

        if (keyPath is null)
            return null;

        var regKey = GetRegistryKey(keyPath);
        return regKey is null ? default : regKey.GetValue(keyName);
    }

    /// <summary>
    /// Retrieves the value associated with the specified registry key.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="path">The path of the registry key.</param>
    /// <returns>The value associated with the specified registry key converted to the specified type, or null if the key does not exist.</returns>
    public T? Get<T>(string path)
    {
        var value = Get(path);
        return value is null ? default : ConvertValue<T>(value);
    }

    /// <inheritdoc/>
    public Dictionary<string, object>? GetRegKeyValues(string path)
    {
        var keys = new Dictionary<string, object>();
        var regKey = GetRegistryKey(path);
        if (regKey is null)
            return keys;

        foreach (var valueName in regKey.GetValueNames())
            keys.Add(valueName, Get(path, valueName)!);

        return keys;
    }

    /// <inheritdoc/>
    public string[]? GetSubKeyNames(string path)
    {
        var regKey = GetRegistryKey(path);

        return regKey?.GetSubKeyNames();
    }

    /// <inheritdoc/>
    public bool TrySet(string path, object value, bool startAdminProcess, RegistryValueKind kind = RegistryValueKind.String)
    {
        var key = Path.GetFileName(path);
        path = Path.GetDirectoryName(path)!;

        if (startAdminProcess && !IsRunningAsAdmin())
            return TrySetKeyWithProcess(Path.Combine(path, key), value, kind);

        var regKey = GetRegistryKey(path, true);
        if (regKey is null)
            return false;

        try
        {
            SetKey(regKey, key, value, kind);
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public void Set(string path, object value, bool startAdminProcess, RegistryValueKind kind = RegistryValueKind.String)
    {
        var key = Path.GetFileName(path);
        path = Path.GetDirectoryName(path)!;

        if (startAdminProcess && !IsRunningAsAdmin())
            TrySetKeyWithProcess(path, value, kind);

        var regKey = GetRegistryKey(path, true) ?? throw new InvalidOperationException("Registry key not found.");
        SetKey(regKey, key, value, kind);
    }

    /// <summary>
    /// Sets the value of a <see cref="RegistryKey"/> path.
    /// </summary>
    /// <param name="registryKey">The <see cref="RegistryKey"/> path.</param>
    /// <param name="key">The key for which to set the value.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="kind">The type of the value.</param>
    private static void SetKey(RegistryKey registryKey, string key, object value, RegistryValueKind kind)
    {
        if (kind == RegistryValueKind.None)
            registryKey.SetValue(key, value);
        else
            registryKey.SetValue(key, value, kind);
    }

    /// <inheritdoc/>
    public bool TrySetKeyWithProcess(string path, object value, RegistryValueKind kind = RegistryValueKind.String)
    {
        var key = Path.GetFileName(path);
        path = Path.GetDirectoryName(path)!;

        registryValueTypesMap.TryGetValue(kind, out var valueType);
        if (valueType is null)
            return false;

        var command = $"reg add \"{path}\" /v \"{key}\" /t {valueType} /d \"{value}\" /f && regedit";
        return StartSetKeyProcess(command);
    }

    private static bool StartSetKeyProcess(string command)
    {
        var procStartInfo = new ProcessStartInfo("cmd", "/c " + command)
        {
            Verb = RunAsVerb,
            UseShellExecute = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        try
        {
            using var process = new Process { StartInfo = procStartInfo };
            process.Start();
            process.WaitForExit(5000);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public bool OpenAt(string path, bool startAdminProcess = true)
    {
        var success = TrySet(LastKey, path, startAdminProcess);
        return success && Open();
    }

    /// <inheritdoc/>
    public bool Open()
    {
        try
        {
            Process.Start("regedit.exe");
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public bool IsRunningAsAdmin()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>
    /// Converts the given object to the type T.
    /// </summary>
    /// <typeparam name="T">The type to which the object should be converted.</typeparam>
    /// <param name="value">The object to convert.</param>
    /// <returns>The object of type T or null if the conversion failed.</returns>
    private static T? ConvertValue<T>(object value)
    {
        try
        {
            return (T?)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            if (!_conversionCache.TryGetValue(typeof(T), out var converter))
            {
                converter = FindConverter<T>(value);
                if (converter is null)
                    return default;

                _conversionCache[typeof(T)] = converter;
            }

            return ((Func<object, T?>)converter)(value);
        }
    }

    /// <summary>
    /// Finds the correct Converter for an object.
    /// </summary>
    /// <typeparam name="T">The type for which to find the converter.</typeparam>
    /// <param name="value">The object for which to find the converter.</param>
    /// <returns>The converter function as a <see cref="Delegate"/>.</returns>
    private static Delegate? FindConverter<T>(object value)
    {
        var targetType = typeof(T);
        var constructor = targetType.GetConstructor([value.GetType()]);
        if (constructor is not null)
            return new Func<object, T?>(val => (T?)constructor.Invoke([val]));

        var parseMethod = targetType.GetMethod("Parse", [typeof(string)]);
        if (parseMethod is not null && parseMethod.IsStatic)
            return new Func<object, T?>(val => (T?)parseMethod.Invoke(null, [val.ToString()]));

        var tryParseMethod = targetType.GetMethod("TryParse", [typeof(string), targetType.MakeByRefType()]);
        if (tryParseMethod is not null && tryParseMethod.IsStatic)
        {
            return new Func<object, T?>(val =>
            {
                object?[] parameters = [val.ToString() ?? string.Empty, null];
                var success = (bool?)tryParseMethod.Invoke(null, parameters);

                return success.HasValue && success.Value ? (T?)parameters[1] : default;
            });
        }

        return null;
    }
}
