using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Events;

namespace ProgramInfos.Manager.Abstractions.Repository;

/// <summary>
/// Handles calling all plugin implementations of <see cref="IProgramInfoDataSourceRepository"/>.
/// </summary>
public interface IProgramInfoDataRepository
{
    /// <summary>
    /// Retrieves all program information from installed programs.
    /// </summary>
    /// <returns>A <see cref="Task"/> that holds an enumerable collection of <see cref="IProgramInfoData"/>.</returns>
    Task<IEnumerable<IProgramInfoData>> GetAll();

    /// <summary>
    /// Writes all the provided program information.
    /// </summary>
    /// <param name="programInfos">The collection of <see cref="IProgramInfoData"/> to write.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task WriteAll(IEnumerable<IProgramInfoData> programInfos);

    /// <summary>
    /// Occurs when a new <see cref="IProgramInfoData"/> is received.
    /// </summary>
    public event ProgramInfoDataReceivedEvent OnProgramInfoDataReceived;

    /// <summary>
    /// Occurs when a <see cref="IProgramInfoData"/> is written.
    /// </summary>
    public event ProgramInfoDataReceivedEvent OnProgramInfoDataWritten;
}
