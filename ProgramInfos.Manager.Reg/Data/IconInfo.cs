using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Reg.Data;

/// <inheritdoc cref="IIconInfo"/>
public sealed class IconInfo : IIconInfo
{
    /// <inheritdoc/>
    public string? Path { get; set; }

    /// <inheritdoc/>
    public int Index { get; set; } = -1;

    /// <inheritdoc/>
    public string? GroupName { get; set; }
}
