using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Service;
using ProgramInfos.Manager.Reg.Data;
using ProgramInfos.Manager.Reg.Extensions;
using System.Text.Json;
using WindowsRegistry.Serializer;

namespace ProgramInfos.Manager.Reg.Service;

public sealed class ProgramInfoDataBackupService : IProgramInfoDataBackupSourceService
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ProgramInfoDataBackupService(JsonSerializerOptions jsonSerializerOptions)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task<bool> CreateBackup(IEnumerable<IProgramInfoData> programInfos, string backupFolderPath)
    {
        try
        {
            var filteredProgramInfos = programInfos.Where(x => x.SourceKey == ProgramInfoData.SourceKeyName);
            var json = JsonSerializer.Serialize(filteredProgramInfos, _jsonSerializerOptions);
            var filename = $"{ProgramInfoData.SourceKeyName.SanitizeFileName().Replace(' ', '-')}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            await File.WriteAllTextAsync(Path.Combine(backupFolderPath, $"{ProgramInfoData.SourceKeyName}.json"), json);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public void DeleteBackup(string backupFolderPath, IProgramInfoDataBackup programInfoDataBackup)
    {
        var backupInfosFilePath = Path.Combine(backupFolderPath, $"{ProgramInfoData.SourceKeyName.SanitizeFileName().Replace(' ', '-')}.json");
        var backUpInfosJson = File.ReadAllText(backupInfosFilePath);
        var backupInfos = JsonSerializer.Deserialize<IEnumerable<IProgramInfoDataBackup>>(backUpInfosJson, _jsonSerializerOptions)?.ToList();
        if (backupInfos is not null && backupInfos.Exists(x => x.Id == programInfoDataBackup.Id))
        {
            backupInfos.Remove(backupInfos.First(x => x.Id == programInfoDataBackup.Id));
            backUpInfosJson = JsonSerializer.Serialize<IEnumerable<IProgramInfoDataBackup>>(backupInfos, _jsonSerializerOptions);
            File.WriteAllText(backupInfosFilePath, backUpInfosJson);
        }

        var backupFilePath = Path.Combine(backupFolderPath, programInfoDataBackup.FileName);
        File.Delete(backupFilePath);
    }

    public async Task<bool> RestoreBackup(string backupFolderPath, IProgramInfoDataBackup programInfoDataBackup)
    {
        try
        {
            var filePath = Path.Combine(backupFolderPath, programInfoDataBackup.FileName);
            var json = File.ReadAllText(filePath);
            var programInfos = JsonSerializer.Deserialize<IEnumerable<ProgramInfoData>>(json, _jsonSerializerOptions);
            if (programInfos is not null)
            {
                RestoreBackup(programInfos);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> RestoreLatestBackup(string backupFolderPath)
    {
        try
        {
            var backupInfosFilePath = Path.Combine(backupFolderPath, $"{ProgramInfoData.SourceKeyName.SanitizeFileName().Replace(' ', '-')}.json");
            var backUpInfosJson = File.ReadAllText(backupInfosFilePath);
            var backupInfos = JsonSerializer.Deserialize<IEnumerable<IProgramInfoDataBackup>>(backUpInfosJson, _jsonSerializerOptions);

            var backupInfo = backupInfos?.OrderByDescending(x => x.BackupDate)?.FirstOrDefault(x => x.SourceKey == ProgramInfoData.SourceKeyName);
            if (backupInfo is not null)
            {
                var json = await File.ReadAllTextAsync(Path.Combine(backupFolderPath, backupInfo.FileName));
                var programInfos = JsonSerializer.Deserialize<IEnumerable<ProgramInfoData>>(json, _jsonSerializerOptions);
                if (programInfos is not null)
                {
                    RestoreBackup(programInfos);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    private static void RestoreBackup(IEnumerable<ProgramInfoData> programInfos)
    {
        foreach (var programInfo in programInfos)
        {
            RegistrySerializer.Serialize(programInfo.RegKey, programInfo);
        }
    }
}
