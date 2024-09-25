using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Repository;
using ProgramInfos.Manager.Reg.Data;
using ProgramInfos.Manager.Reg.Extensions;
using System.Text.Json;

namespace ProgramInfos.Manager.Reg.Repository;

/// <inheritdoc />
public sealed class ProgramInfoDataBackupRepository : IProgramInfoDataBackupSourceRepository
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramInfoDataBackupRepository"/> class with the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="jsonSerializerOptions">The <see cref="JsonSerializerOptions"/> to use.</param>
    public ProgramInfoDataBackupRepository(JsonSerializerOptions jsonSerializerOptions)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    /// <inheritdoc />
    public IEnumerable<IProgramInfoDataBackup> GetBackupInfos(string backupFolderPath)
    {
        var filePath = Path.Combine(backupFolderPath, $"{ProgramInfoData.SourceKeyName.SanitizeFileName().Replace(' ', '-')}.json");
        var json = File.ReadAllText(filePath);
        var backups = JsonSerializer.Deserialize<IEnumerable<IProgramInfoDataBackup>>(json, _jsonSerializerOptions);
        return backups ?? [];
    }
}
