using Programs.Manager.Common.Win.Data;

namespace Programs.Manager.Common.Win.Repository.ProgramInfo;

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
