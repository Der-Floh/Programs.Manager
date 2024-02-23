namespace Programs.Manager.Reader.Win.Data;

/// <summary>
/// Represents an icon index.
/// </summary>
public sealed class IconIndex
{
    /// <summary>
    /// The index of the icon.
    /// </summary>
    public int Index { get; set; } = -1;

    /// <summary>
    /// The groupname of the icon.
    /// </summary>
    public string? GroupName { get; set; }
}
