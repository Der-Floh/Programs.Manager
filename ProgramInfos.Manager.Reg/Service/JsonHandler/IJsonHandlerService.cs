
namespace ProgramInfos.Manager.Reg.Service.JsonHandler;

public interface IJsonHandlerService<T>
{
    IEnumerable<T>? Deserialize(string jsonString);
    string? Serialize(IEnumerable<T> items);
}