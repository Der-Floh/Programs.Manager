namespace ProgramInfos.Manager.Abstractions.Events;

/// <summary>
/// Event that is fired when a <see cref="IProgramInfoData"/> is received.
/// </summary>
/// <param name="sender">The event sender object.</param>
/// <param name="args">The <see cref="ProgramInfoDataEventArgs"/> event args.</param>
public delegate void ProgramInfoDataReceivedEvent(object sender, ProgramInfoDataEventArgs args);
