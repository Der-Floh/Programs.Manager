using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Abstractions.Repository;

/// <summary>
/// Interface for plugins to implement the repository for <see cref="IProgramInfoDataBackup"/>
/// </summary>
public interface IProgramInfoDataBackupSourceRepository
{
    /// <summary>
    /// Retrieves all backup infos from the given backup folder
    /// </summary>
    /// <param name="backupFolderPath">The path to the backup folder</param>
    /// <returns>An IEnumerable of <see cref="IProgramInfoDataBackup"/></returns>
    IEnumerable<IProgramInfoDataBackup> GetBackupInfos(string backupFolderPath);
}
