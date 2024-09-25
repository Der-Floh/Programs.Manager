using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Repository;
using ProgramInfos.Manager.Reg.Data;
using ProgramInfos.Manager.Reg.Extensions;
using System.Text.Json;

namespace ProgramInfos.Manager.Reg.Repository;

public sealed class ProgramInfoDataBackupRepository : IProgramInfoDataBackupSourceRepository
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ProgramInfoDataBackupRepository(JsonSerializerOptions jsonSerializerOptions)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public IEnumerable<IProgramInfoDataBackup> GetBackupInfos(string backupPath)
    {
        var filePath = Path.Combine(backupPath, $"{ProgramInfoData.SourceKeyName.SanitizeFileName().Replace(' ', '-')}.json");
        var json = File.ReadAllText(filePath);
        var backups = JsonSerializer.Deserialize<IEnumerable<IProgramInfoDataBackup>>(json, _jsonSerializerOptions);
        return backups ?? [];
    }
}
