using Programs.Manager.Common.Win.Data;
using System.Drawing;

namespace Programs.Manager.Common.Win.Service;

/// <summary>
/// Service for managing operations related to program registry information.
/// </summary>
public interface IProgramRegInfoService
{
    /// <summary>
    /// Gets the icon associated with the program.
    /// </summary>
    /// <returns>A Bitmap that represents the Icon.</returns>
    Bitmap? GetIcon(ProgramRegInfoData programRegInfoData);
}
