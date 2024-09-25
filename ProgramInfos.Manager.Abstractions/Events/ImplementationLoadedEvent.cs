namespace ProgramInfos.Manager.Abstractions.Events;

/// <summary>
/// Event that is fired when a interface is implemented.
/// </summary>
/// <param name="sender">The event sender object.</param>
/// <param name="args">The <see cref="ImplementationLoadedEventArgs"/> event args.</param>
public delegate void ImplementationLoadedEvent(object sender, ImplementationLoadedEventArgs args);

public sealed class ImplementationLoadedEventArgs : EventArgs
{
    /// <summary>
    /// The interface for which the implementation was loaded.
    /// </summary>
    public Type? LoadedInterface { get; set; }

    /// <summary>
    /// The implementation that was loaded.
    /// </summary>
    public Type? LoadedImplementation { get; set; }
}
