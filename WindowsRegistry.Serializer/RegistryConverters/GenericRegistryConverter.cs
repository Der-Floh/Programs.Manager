namespace WindowsRegistry.Serializer.RegistryConverters;

using System;
using System.Collections.Concurrent;

public class GenericRegistryConverter : RegistryConverter
{
    private const string ParseMethodeName = "Parse";
    private const string TryParseMethodeName = "TryParse";
    private static readonly ConcurrentDictionary<Type, Delegate> _conversionCache = new();

    public override bool TryRead(ref object registryData, Type typeToConvert, out object? result)
    {
        result = ConvertValue(ref registryData, typeToConvert);
        return result is not null;
    }

    public override bool CanConvert(Type typeToConvert) => true;

    private static object? ConvertValue(ref object value, Type targetType)
    {
        try
        {
            return Convert.ChangeType(value, targetType);
        }
        catch
        {
            if (!_conversionCache.TryGetValue(targetType, out var converter))
            {
                converter = FindConverter(ref value, targetType);
                if (converter is null)
                    return null;

                _conversionCache[targetType] = converter;
            }

            return converter.DynamicInvoke(value);
        }
    }

    private static Delegate? FindConverter(ref object value, in Type targetType)
    {
        var constructor = targetType.GetConstructor([value.GetType()]);
        if (constructor is not null)
            return new Func<object, object?>(val => constructor.Invoke([val]));

        var parseMethod = targetType.GetMethod(ParseMethodeName, [typeof(string)]);
        if (parseMethod is not null && parseMethod.IsStatic)
            return new Func<object, object?>(val => parseMethod.Invoke(null, new[] { val.ToString() }));

        var tryParseMethod = targetType.GetMethod(TryParseMethodeName, [typeof(string), targetType.MakeByRefType()]);
        if (tryParseMethod is not null && tryParseMethod.IsStatic)
        {
            return new Func<object, object?>(val =>
            {
                object?[] parameters = [val.ToString() ?? string.Empty, null];
                var success = (bool?)tryParseMethod.Invoke(null, parameters);

                if (success.HasValue && success.Value)
                    return parameters[1];

                return null;
            });
        }

        return null;
    }
}
