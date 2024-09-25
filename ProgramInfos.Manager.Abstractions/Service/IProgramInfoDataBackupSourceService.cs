using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Abstractions.Service;

/// <summary>
/// Interface for plugins to implement the service for backuping <see cref="IProgramInfoData"/>
/// </summary>
public interface IProgramInfoDataBackupSourceService
{
    /// <summary>
    /// Creates a backup of the given collection of <see cref="IProgramInfoData"/>.
    /// </summary>
    /// <param name="programInfos">A collection of <see cref="IProgramInfoData"/>s to backup.</param>
    /// <param name="backupFolderPath">The folder path where the backup should be created.</param>
    /// <returns>True if the backup was created successfully, false otherwise.</returns>
    Task<bool> CreateBackup(IEnumerable<IProgramInfoData> programInfos, string backupFolderPath);

    /// <summary>
    /// Deletes a backup with the given path.
    /// </summary>
    /// <param name="backupFolderPath">The path of the backup to delete.</param>
    /// <param name="programInfoDataBackup">The <see cref="IProgramInfoDataBackup"/> to delete.</param>
    void DeleteBackup(string backupFolderPath, IProgramInfoDataBackup programInfoDataBackup);

    /// <summary>
    /// Restores the backup of the given backup file.
    /// </summary>
    /// <param name="backupFolderPath">The path of the backup file to restore.</param>
    /// <returns>True if the backup was restored successfully, false otherwise.</returns>
    Task<bool> RestoreBackup(string backupFolderPath, IProgramInfoDataBackup programInfoDataBackup);

    /// <summary>
    /// Restores the latest backup.
    /// </summary>
    /// <param name="backupFolderPath">The path of the backup folder in which the latest backup is stored.</param>
    /// <returns>True if the latest backup was restored successfully, false otherwise.</returns>
    Task<bool> RestoreLatestBackup(string backupFolderPath);
}
