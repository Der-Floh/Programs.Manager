namespace ProgramInfos.Manager.Abstractions.Data;

public interface IIconInfo
{
    /// <summary>
    /// Path to the icon.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Index of the icon.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Name of the icon group in which the index is located. When empty, the index is global.
    /// </summary>
    public string? GroupName { get; set; }
}
