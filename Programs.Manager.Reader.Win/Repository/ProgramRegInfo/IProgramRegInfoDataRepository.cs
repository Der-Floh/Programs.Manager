using Programs.Manager.Common.Data;

namespace Programs.Manager.Reader.Win.Repository.ProgramRegInfo;

/// <summary>
/// Repository for accessing and processing registry information of installed programs.
/// </summary>
public interface IProgramRegInfoDataRepository
{
    /// <summary>
    /// Constructs a <see cref="ProgramRegInfoData"/> object from registry key values.
    /// </summary>
    /// <param name="keys">A dictionary containing the registry key values.</param>
    /// <param name="regKey">The registry key path.</param>
    /// <returns>A <see cref="ProgramRegInfoData"/> object populated with data from the registry.</returns>
    ProgramRegInfoData Get(Dictionary<string, object?> keys, string regKey);

    /// <summary>
    /// Retrieves all program information from the registry.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="ProgramRegInfoData"/>.</returns>
    IEnumerable<ProgramRegInfoData> GetAll();
}
