namespace WindowsRegistry.Serializer.RegistryConverters;

public class RegistryIntConverter : RegistryConverter<int>
{
    public override bool TryRead(ref object registryData, out int result)
    {
        if (registryData is int intValue)
        {
            result = intValue;
            return true;
        }

        return int.TryParse(registryData.ToString(), out result);
    }
}