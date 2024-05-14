using System.Reflection;
using WindowsRegistry.Serializer.Attributes;
using WindowsRegistry.Serializer.Data;
using WindowsRegistry.Serializer.Services;

namespace WindowsRegistry.Serializer;

public static class RegistrySerializer
{
    private static readonly RegistryService _registryService = new();
    private static readonly Dictionary<Type, List<PropertyCacheInfo>> _cache = [];

    public static async Task<IEnumerable<T>?> DeserializeDirectorysAsync<T>(string rootDirectoryPath, RegistrySerializerOptions? options = null) where T : class
    {
        options ??= RegistrySerializerOptions.Default;
        var programs = _registryService.GetSubKeyNames(rootDirectoryPath);
        if (programs is null || programs.Length == 0)
            return null;

        var tasks = new List<Task<T?>>();

        foreach (var program in programs)
            tasks.Add(Task.Run(() => Deserialize<T>(Path.Combine(rootDirectoryPath, program), options)));

        var results = await Task.WhenAll(tasks);

        return results.Where(r => r is not null)!;
    }

    public static IEnumerable<T>? DeserializeDirectorys<T>(string rootDirectoryPath, RegistrySerializerOptions? options = null) where T : class
    {
        options ??= RegistrySerializerOptions.Default;
        var programs = _registryService.GetSubKeyNames(rootDirectoryPath);
        if (programs is null || programs.Length == 0)
            return null;

        var results = new List<T?>();

        foreach (var program in programs)
            results.Add(Deserialize<T>(Path.Combine(rootDirectoryPath, program), options));

        return results.Where(r => r is not null)!;
    }

    public static T? Deserialize<T>(string path, RegistrySerializerOptions? options = null) where T : class
    {
        uint successfulyDeserializedProperties = 0;
        options ??= RegistrySerializerOptions.Default;

        Type type = typeof(T);
        var classPropertyInfos = GetPropertyCacheInfo(type);

        T obj = (T)Activator.CreateInstance(type)!;

        foreach (var propertyInfo in classPropertyInfos)
        {
            if (!propertyInfo.Property.CanWrite || propertyInfo.Ignore)
                continue;

            foreach (var name in propertyInfo.RegistryNames)
            {
                var registryKeyName = Path.Combine(path, name);
                object? propertyValue = null;

                if (propertyInfo.Converter is not null)
                {
                    if (GetPropertyValue(propertyInfo, propertyInfo.Converter, registryKeyName, options, ref propertyValue))
                        successfulyDeserializedProperties++;
                }
                else
                {
                    var converter = options.GetConverter(propertyInfo);
                    if (GetPropertyValue(propertyInfo, converter, registryKeyName, options, ref propertyValue))
                        successfulyDeserializedProperties++;
                }

                if (propertyValue is not null)
                {
                    if (propertyInfo.PostProcess is not null)
                        propertyValue = propertyInfo.PostProcess.Effect(ref propertyValue);

                    propertyInfo.Property.SetValue(obj, propertyValue);
                    break;
                }
            }
        }

        if (successfulyDeserializedProperties == 0)
            return default;

        return obj;
    }

    private static List<PropertyCacheInfo> GetPropertyCacheInfo(Type type)
    {
        if (_cache.TryGetValue(type, out var list))
            return list;

        var properties = new List<PropertyCacheInfo>();

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var ignoreAttribute = property.GetCustomAttribute<RegistryIgnoreAttribute>();
            var attribute = property.GetCustomAttribute<RegistryNamesAttribute>();
            var names = attribute?.Names ?? [property.Name];

            RegistryConverter? converter = null;
            var converterAttribute = property.GetCustomAttribute<RegistryConverterAttribute>();
            if (converterAttribute is not null)
                converter = (RegistryConverter?)Activator.CreateInstance(converterAttribute.ConverterType);

            RegistryDeserializerPostProcess? postProcess = null;
            var postProcessAttribute = property.GetCustomAttribute<RegistryDeserializerPostProcessAttribute>();
            if (postProcessAttribute is not null)
                postProcess = (RegistryDeserializerPostProcess?)Activator.CreateInstance(postProcessAttribute.ConverterType);

            properties.Add(new PropertyCacheInfo(property, names)
            {
                Converter = converter,
                Ignore = ignoreAttribute is not null,
                PostProcess = postProcess
            });
        }

        _cache[type] = properties;

        return properties;
    }

    private static bool GetPropertyValue(PropertyCacheInfo propertyInfo, RegistryConverter converter, string registryKeyName, RegistrySerializerOptions options, ref object? propertyValue)
    {
        propertyValue = null;

        var objectValue = _registryService.Get(registryKeyName);
        if (objectValue is null)
            return false;

        if (options.IgnoreThrownExceptions)
        {
            try
            {
                return ReadPropertyFromConverter(ref objectValue, propertyInfo, converter, ref propertyValue);
            }
            catch
            {
                propertyValue = null;
                return false;
            }
        }
        else
        {
            return ReadPropertyFromConverter(ref objectValue, propertyInfo, converter, ref propertyValue);
        }
    }

    private static bool ReadPropertyFromConverter(ref object objectValue, PropertyCacheInfo propertyInfo, RegistryConverter converter, ref object? propertyValue)
    {
        bool success = converter.TryRead(ref objectValue, propertyInfo.Property.PropertyType, out propertyValue);
        if (!success)
            propertyValue = GetPropertyDefault(propertyInfo);

        return success;
    }

    private static object? GetPropertyDefault(PropertyCacheInfo propertyInfo)
    {
        if (propertyInfo.UnderlyingTyp is not null && propertyInfo.UnderlyingTyp.IsValueType)
        {
            if (propertyInfo.EmptyConstructorInfo is not null)
                return propertyInfo.EmptyConstructorInfo.Invoke(null);

            return Activator.CreateInstance(propertyInfo.UnderlyingTyp);
        }
        else
            return null;
    }
}
