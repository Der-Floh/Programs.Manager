namespace ProgramInfos.Manager.Abstractions.Data;

public interface IProgramInfoDataBackup
{
    Guid Id { get; set; }
    string SourceKey { get; set; }
    int ProgramAmount { get; set; }
    DateTime BackupDate { get; set; }
    string FileName { get; set; }
}
