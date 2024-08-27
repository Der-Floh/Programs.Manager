using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Abstractions.Events;

/// <summary>
/// Event args that contain a <see cref="IProgramInfoData"/>.
/// </summary>
public class ProgramInfoDataEventArgs : EventArgs
{
    /// <summary>
    /// The <see cref="IProgramInfoData"/> that is given.
    /// </summary>
    public IProgramInfoData? ProgramInfoData { get; set; }
}
