namespace WindowsRegistry.Serializer.RegistryConverters;

public class RegistryVersionConverter : RegistryConverter<Version>
{

    public override bool TryRead(ref object registryData, out Version? result)
    {
        if (registryData is string registryValue)
        {
            if (int.TryParse(registryValue, out int versionInt))
            {
                result = ConvertToVersion((uint)versionInt);
                return result is not null;
            }

            return Version.TryParse(registryValue, out result);
        }

        result = null;
        return false;
    }

    private static Version ConvertToVersion(uint versionUint)
    {
        int major = (int)(versionUint >> 24);
        int minor = (int)((versionUint >> 16) & 0xFF);
        int build = (int)((versionUint >> 8) & 0xFF);
        int revision = (int)(versionUint & 0xFF);
        return new Version(major, minor, build, revision);
    }



    //private static Version ConvertToVersion(uint versionUint)
    //{
    //    if (versionUint > 99999999)
    //    {
    //        int major = (int)((versionUint >> 24) & 0xFF);
    //        int minor = (int)((versionUint >> 16) & 0xFF);
    //        int build = (int)((versionUint >> 8) & 0xFF);
    //        int revision = (int)(versionUint & 0xFF);
    //        return new Version(major, minor, build, revision);
    //    }
    //    else if (versionUint > 9999)
    //    {
    //        int major = (int)(versionUint / 10000);
    //        int minor = (int)((versionUint / 100) % 100);
    //        int build = (int)(versionUint % 100);
    //        return new Version(major, minor, build);
    //    }
    //    else
    //    {
    //        int major = (int)(versionUint / 100);
    //        int minor = (int)(versionUint % 100);
    //        return new Version(major, minor);
    //    }
    //}
}