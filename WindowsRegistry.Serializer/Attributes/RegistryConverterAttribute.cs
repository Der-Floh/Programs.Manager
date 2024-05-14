namespace WindowsRegistry.Serializer.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RegistryConverterAttribute(Type converterType) : Attribute
{
    public Type ConverterType { get; } = converterType;
}