using ProgramInfos.Manager.Abstractions.Data;

namespace ProgramInfos.Manager.Abstractions.Repository;

public interface IProgramInfoDataBackupSourceRepository
{
    IEnumerable<IProgramInfoDataBackup> GetBackupInfos(string backupPath);
}
