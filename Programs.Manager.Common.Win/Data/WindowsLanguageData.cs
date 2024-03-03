namespace Programs.Manager.Common.Win.Data;

/// <summary>
/// Represents the language data for a Windows operating system.
/// </summary>
public sealed class WindowsLanguageData
{
    /// <summary>
    /// Gets or sets the human-readable name of the language.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the standard language code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the Locale ID (LCID) code for the language.
    /// </summary>
    public string? LCIDCode { get; set; }

    /// <summary>
    /// Gets or sets the Windows-specific decimal code for the language.
    /// </summary>
    /// <remarks>This is typically used in Windows API calls.</remarks>
    public uint WindowsCodeDecimal { get; set; }

    /// <summary>
    /// Gets or sets the Windows-specific hexadecimal code for the language.
    /// </summary>
    /// <remarks>Often used in registry settings or system configuration.</remarks>
    public string? WindowsCodeHex { get; set; }

    /// <summary>
    /// Gets or sets the code page number used for character encoding in the language.
    /// </summary>
    public uint CodePage { get; set; }

    public override string? ToString() => Name;

    /// <summary>
    /// Converts the language data to CSV format.
    /// </summary>
    /// <returns>A string representation of the language data in CSV format.</returns>
    public string? ToCsv() => $"{Name};{Code};{LCIDCode};{WindowsCodeDecimal};{WindowsCodeHex};{CodePage}";
}
