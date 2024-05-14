using System.Reflection;

namespace WindowsRegistry.Serializer.Data;
public class PropertyCacheInfo
{
    public PropertyCacheInfo(PropertyInfo property, string[] registryNames)
    {
        Property = property;
        RegistryNames = registryNames;
        UnderlyingTyp = Nullable.GetUnderlyingType(property.PropertyType);
        if (UnderlyingTyp is not null)
        {
            try
            {
                EmptyConstructorInfo = UnderlyingTyp.GetConstructor(Type.EmptyTypes);
            }
            catch
            {
                EmptyConstructorInfo = null;
            }
        }
    }

    public PropertyInfo Property { get; private set; }
    public Type? UnderlyingTyp { get; private set; }
    public string[] RegistryNames { get; private set; }
    public ConstructorInfo? EmptyConstructorInfo { get; private set; }
    public RegistryConverter? Converter { get; set; }
    public RegistryDeserializerPostProcess? PostProcess { get; set; }
    public bool Ignore { get; set; }
}
