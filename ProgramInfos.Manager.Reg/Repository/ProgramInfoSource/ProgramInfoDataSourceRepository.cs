using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Events;
using ProgramInfos.Manager.Abstractions.Repository;

namespace ProgramInfos.Manager.Reg.Repository.ProgramInfoSource;

/// <inheritdoc cref="IProgramInfoDataSourceRepository"/>
public sealed class ProgramInfoDataSourceRepository : IProgramInfoDataSourceRepository
{
    private readonly ProgramInfo.IProgramInfoDataRepository _programInfoDataRepository;

    /// <summary>
    /// Creates a new instance of <see cref="ProgramInfoDataSourceRepository"/>
    /// </summary>
    /// <param name="programInfoDataRepository">The <see cref="ProgramInfo.IProgramInfoDataRepository"/> to use for getting and writing <see cref="IProgramInfoData"/></param>
    public ProgramInfoDataSourceRepository(ProgramInfo.IProgramInfoDataRepository programInfoDataRepository)
    {
        _programInfoDataRepository = programInfoDataRepository;
    }

    /// <inheritdoc />
    public Task<IEnumerable<IProgramInfoData>> GetAll(ProgramInfoDataReceivedEvent? OnProgramInfoDataReceived = null) => _programInfoDataRepository.GetAll(OnProgramInfoDataReceived);

    /// <inheritdoc />
    public Task WriteAll(IEnumerable<IProgramInfoData> programInfos, ProgramInfoDataReceivedEvent? OnProgramInfoDataWritten = null) => throw new NotImplementedException();
}
