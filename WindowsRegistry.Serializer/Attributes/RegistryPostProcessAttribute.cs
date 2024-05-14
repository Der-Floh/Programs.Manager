namespace WindowsRegistry.Serializer.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RegistryDeserializerPostProcessAttribute(Type converterType) : Attribute
{
    public Type ConverterType { get; } = converterType;
}