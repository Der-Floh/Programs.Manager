using Programs.Manager.Reader.Win.Data;

namespace Programs.Manager.Reader.Win.Service.WindowsLanguage;

/// <summary>
/// Service for managing Windows language data.
/// </summary>
public interface IWindowsLanguageService
{
    /// <summary>
    /// Retrieves all language data.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="WindowsLanguageData"/>, or null if the data cannot be loaded.</returns>
    IEnumerable<WindowsLanguageData>? GetAll();
}
