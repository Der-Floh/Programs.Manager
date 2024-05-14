using System.Globalization;

namespace WindowsRegistry.Serializer.RegistryConverters;

public class RegistryDateTimeConverter : RegistryConverter<DateTime>
{
    private static readonly string[] _formats =
    [
        "M/d/yyyy", "MM/dd/yyyy",
        "d/M/yyyy", "dd/MM/yyyy",
        "yyyy/M/d", "yyyy/MM/dd",
        "M-d-yyyy", "MM-dd-yyyy",
        "d-M-yyyy", "dd-MM-yyyy",
        "yyyy-M-d", "yyyy-MM-dd",
        "M.d.yyyy", "MM.dd.yyyy",
        "d.M.yyyy", "dd.MM.yyyy",
        "yyyy.M.d", "yyyy.MM.dd",
        "M,d,yyyy", "MM,dd,yyyy",
        "d,M,yyyy", "dd,MM,yyyy",
        "yyyy,M,d", "yyyy,MM,dd",
        "M d yyyy", "MM dd yyyy",
        "d M yyyy", "dd MM yyyy",
        "yyyy M d", "yyyy MM dd"
    ];

    public override bool TryRead(ref object registryData, out DateTime result)
    {
        string registryValue = registryData.ToString() ?? string.Empty;

        if (registryValue.Length == 8 &&
            DateTime.TryParseExact(registryValue, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            result = date;
            return true;
        }

        foreach (string format in _formats)
        {
            if (format.Length == registryValue.Length &&
                DateTime.TryParseExact(registryValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                result = date;
                return true;
            }
        }

        if (DateTime.TryParse(registryValue, out DateTime fallbackDate))
        {
            result = fallbackDate;
            return true;
        }

        result = default;
        return false;
    }
}