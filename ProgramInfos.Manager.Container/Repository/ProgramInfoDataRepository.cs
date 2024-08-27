using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Events;
using ProgramInfos.Manager.Abstractions.Repository;

namespace ProgramInfos.Manager.Container.Repository;

/// <inheritdoc cref="IProgramInfoDataRepository"/>
public class ProgramInfoDataRepository : IProgramInfoDataRepository
{
    /// <inheritdoc/>
    public event ProgramInfoDataReceivedEvent? OnProgramInfoDataReceived;

    /// <inheritdoc/>
    public event ProgramInfoDataReceivedEvent? OnProgramInfoDataWritten;

    private readonly IEnumerable<IProgramInfoDataSourceRepository> _programInfoDataSourceRepositories;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoDataRepository"/> class.
    /// </summary>
    /// <param name="programInfoDataSourceRepositories"> An IEnumerable of <see cref="IProgramInfoDataSourceRepository"/> for all plugins.</param>
    public ProgramInfoDataRepository(IEnumerable<IProgramInfoDataSourceRepository> programInfoDataSourceRepositories)
    {
        _programInfoDataSourceRepositories = programInfoDataSourceRepositories;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IProgramInfoData>> GetAll()
    {
        var tasks = new List<Task<IEnumerable<IProgramInfoData>>>();
        foreach (var programInfoDataSourceRepository in _programInfoDataSourceRepositories)
        {
            tasks.Add(programInfoDataSourceRepository.GetAll(OnProgramInfoDataReceived));
        }

        return (await Task.WhenAll(tasks)).SelectMany(x => x);
    }

    /// <inheritdoc/>
    public async Task WriteAll(IEnumerable<IProgramInfoData> programInfos)
    {
        var tasks = new List<Task>();
        foreach (var programInfoDataSourceRepository in _programInfoDataSourceRepositories)
        {
            tasks.Add(programInfoDataSourceRepository.WriteAll(programInfos, OnProgramInfoDataWritten));
        }

        await Task.WhenAll(tasks);
    }
}
