using WindowsRegistry.Serializer.RegistryConverters;

namespace WindowsRegistry.Serializer.Data;
public class RegistrySerializerOptions
{
    private static readonly GenericRegistryConverter _genericRegistryConverter = new();
    private readonly List<RegistryConverter> _converters = [];

    public IList<RegistryConverter> Converters => _converters;
    public bool IgnoreThrownExceptions { get; set; }

    public void AddConverter<T>(RegistryConverter<T> converter) => _converters.Add(converter);

    public RegistryConverter GetConverter(PropertyCacheInfo propertyCacheInfo)
    {
        Type underlyingType = propertyCacheInfo.UnderlyingTyp ?? propertyCacheInfo.Property.PropertyType;

        foreach (var converter in _converters)
        {
            if (converter.CanConvert(underlyingType))
                return converter;
        }

        return _genericRegistryConverter;
    }

    private static RegistrySerializerOptions? _default;
    public static RegistrySerializerOptions Default
    {
        get
        {
            if (_default is null)
            {
                _default = new RegistrySerializerOptions() { IgnoreThrownExceptions = true };
                _default.Converters.Add(new RegistryDateTimeConverter());
                _default.Converters.Add(new RegistryIntConverter());
                _default.Converters.Add(new RegistryStringConverter());
                _default.Converters.Add(new RegistryVersionConverter());
            }

            return _default;
        }
    }

}
