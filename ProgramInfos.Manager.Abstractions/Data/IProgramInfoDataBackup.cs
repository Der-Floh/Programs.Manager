namespace ProgramInfos.Manager.Abstractions.Data;

public interface IProgramInfoDataBackup
{
    /// <summary>
    /// The unique identifier of the backup.
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// The source key of the plugin that created the backup.
    /// </summary>
    string SourceKey { get; set; }

    /// <summary>
    /// The amount of programs in the backup.
    /// </summary>
    int ProgramAmount { get; set; }

    /// <summary>
    /// The creation date of the backup.
    /// </summary>
    DateTime BackupDate { get; set; }

    /// <summary>
    /// The full path to the backup file.
    /// </summary>
    string FileName { get; set; }
}
