using System.Text.Json;

namespace ProgramInfos.Manager.Reg.Service.JsonHandler;

public sealed class JsonHandlerService<T> : IJsonHandlerService<T>
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
    };

    public string? Serialize(IEnumerable<T> items)
    {
        try
        {
            return JsonSerializer.Serialize(items, _options);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public IEnumerable<T>? Deserialize(string jsonString)
    {
        try
        {
            return JsonSerializer.Deserialize<IEnumerable<T>>(jsonString, _options);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
