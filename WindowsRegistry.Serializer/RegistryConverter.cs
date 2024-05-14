namespace WindowsRegistry.Serializer;

public abstract class RegistryConverter
{
    public abstract bool TryRead(ref object registryData, Type typeToConvert, out object? result);
    public abstract bool CanConvert(Type typeToConvert);
}

public abstract class RegistryConverter<T> : RegistryConverter
{
    public abstract bool TryRead(ref object registryData, out T? result);

    public override bool TryRead(ref object registryData, Type typeToConvert, out object? result)
    {
        bool success = TryRead(ref registryData, out T? typedResult);
        result = typedResult;
        return success;
    }

    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(T);
}
