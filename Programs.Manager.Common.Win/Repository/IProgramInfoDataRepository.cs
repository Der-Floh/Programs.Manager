using Programs.Manager.Common.Win.Data;

namespace Programs.Manager.Common.Win.Repository;

public delegate void ProgramInfoDataReceivedEvent(object sender, ProgramInfoDataEventArgs args);

public class ProgramInfoDataEventArgs : EventArgs
{
    public ProgramInfoData ProgramInfoData { get; set; }
}

/// <summary>
/// Repository for retrieving and processing information about installed programs.
/// </summary>
public interface IProgramInfoDataRepository
{
    /// <summary>
    /// Retrieves all program information and optionally performs an action on each item.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="ProgramInfoData"/>.</returns>
    IEnumerable<ProgramInfoData> GetAll();

    /// <summary>
    /// Occurs when a new <see cref="ProgramInfoData"/> is received.
    /// </summary>
    public event ProgramInfoDataReceivedEvent OnProgramInfoDataReceived;
}
