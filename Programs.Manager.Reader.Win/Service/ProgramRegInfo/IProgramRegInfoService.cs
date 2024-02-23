using Programs.Manager.Reader.Win.Data;
using System.Drawing;

namespace Programs.Manager.Reader.Win.Service.ProgramRegInfo;

/// <summary>
/// Service for managing operations related to program registry information.
/// </summary>
public interface IProgramRegInfoService
{
    /// <summary>
    /// Fetches fallback values for the program.
    /// </summary>
    void FetchFallbackValues(ProgramRegInfoData programRegInfoData);

    /// <summary>
    /// Gets the icon associated with the program.
    /// </summary>
    /// <returns>A Bitmap that represents the Icon.</returns>
    Bitmap? GetIcon(ProgramRegInfoData programRegInfoData);
}
