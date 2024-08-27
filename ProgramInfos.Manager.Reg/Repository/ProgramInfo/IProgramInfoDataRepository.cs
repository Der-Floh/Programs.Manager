using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Events;

namespace ProgramInfos.Manager.Reg.Repository.ProgramInfo;

/// <summary>
/// Handles the logic for getting all program information.
/// </summary>
public interface IProgramInfoDataRepository
{
    /// <summary>
    /// Gets all program information from the registry.
    /// </summary>
    /// <param name="OnProgramInfoDataReceived">The <see cref="ProgramInfoDataReceivedEvent"/> event to be triggered foreach program information received.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation and holds an IEnumerable of <see cref="IProgramInfoData"/>.</returns>
    Task<IEnumerable<IProgramInfoData>> GetAll(ProgramInfoDataReceivedEvent? OnProgramInfoDataReceived = null);
}