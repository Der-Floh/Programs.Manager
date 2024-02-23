using Programs.Manager.Reader.Win.Data;

namespace Programs.Manager.Reader.Win.Repository.ProgramInfo;

/// <summary>
/// Repository for retrieving and processing information about installed programs.
/// </summary>
public interface IProgramInfoRepository
{
    /// <summary>
    /// Retrieves all program information and optionally performs an action on each item.
    /// </summary>
    /// <param name="action">An optional action to perform on each <see cref="ProgramRegInfoData"/> item.</param>
    /// <returns>An enumerable collection of <see cref="ProgramInfoData"/>.</returns>
    IEnumerable<ProgramInfoData> GetAll(Action<ProgramRegInfoData>? action = null);
}
