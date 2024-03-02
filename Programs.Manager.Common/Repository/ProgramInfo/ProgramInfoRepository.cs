using Programs.Manager.Common.Data;
using Programs.Manager.Common.Repository.ProgramInfo;

namespace Programs.Manager.Common.Repository.ProgramInfoRepository;

public class ProgramInfoRepository : IProgramInfoRepository
{
    private readonly IEnumerable<IProgramInfoDataRepository> _programInfoDataRepositories;

    public ProgramInfoRepository(IEnumerable<IProgramInfoDataRepository> programInfoDataRepositories)
    {
        _programInfoDataRepositories = programInfoDataRepositories;
    }

    public IEnumerable<ProgramInfoData> GetAll(Action<ProgramRegInfoData>? action = null)
    {
        var result = new List<ProgramInfoData>();
        foreach (var repository in _programInfoDataRepositories)
        {
            result.AddRange(repository.GetAll(action));
        }

        return result;
    }
}
