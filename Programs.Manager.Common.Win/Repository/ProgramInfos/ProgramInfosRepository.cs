using Programs.Manager.Common.Win.Data;

namespace Programs.Manager.Common.Win.Repository.ProgramInfos;

public class ProgramInfosRepository : IProgramInfosRepository
{
    private readonly IEnumerable<IProgramInfoDataRepository> _programInfoDataRepositories;
    public event ProgramInfoDataReceivedEvent OnProgramInfoDataReceived;

    public ProgramInfosRepository(IEnumerable<IProgramInfoDataRepository> programInfoDataRepositories)
    {
        _programInfoDataRepositories = programInfoDataRepositories;
        foreach (var repository in _programInfoDataRepositories)
        {
            repository.OnProgramInfoDataReceived += (s, e) => OnProgramInfoDataReceived?.Invoke(this, e);
        }
    }

    public IEnumerable<ProgramInfoData> GetAll()
    {
        var result = new List<ProgramInfoData>();
        foreach (var repository in _programInfoDataRepositories)
        {
            result.AddRange(repository.GetAll());
        }

        return result;
    }
}
