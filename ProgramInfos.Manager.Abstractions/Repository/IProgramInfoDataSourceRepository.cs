using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Events;

namespace ProgramInfos.Manager.Abstractions.Repository;

/// <summary>
/// Interface for plugins to implement the repository for <see cref="IProgramInfoData"/>
/// </summary>
public interface IProgramInfoDataSourceRepository
{
    /// <summary>
    /// Gets all <see cref="IProgramInfoData"/>.
    /// </summary>
    /// <param name="OnProgramInfoDataReceived">The <see cref="ProgramInfoDataReceivedEvent"/> to be invoked.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation and holds the <see cref="IProgramInfoData"/>.</returns>
    Task<IEnumerable<IProgramInfoData>> GetAll(ProgramInfoDataReceivedEvent? OnProgramInfoDataReceived = null);

    /// <summary>
    /// Writes all <see cref="IProgramInfoData"/>.
    /// </summary>
    /// <param name="programInfos">The <see cref="IProgramInfoData"/> to be written.</param>
    /// <param name="OnProgramInfoDataWritten">The <see cref="ProgramInfoDataReceivedEvent"/> to be invoked.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task WriteAll(IEnumerable<IProgramInfoData> programInfos, ProgramInfoDataReceivedEvent? OnProgramInfoDataWritten = null);
}
