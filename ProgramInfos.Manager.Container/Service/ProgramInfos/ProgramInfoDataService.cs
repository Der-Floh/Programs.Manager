using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Service;

namespace ProgramInfos.Manager.Container.Service.ProgramInfos;

/// <inheritdoc cref="IProgramInfoDataService"/>
public sealed class ProgramInfoDataService : IProgramInfoDataService
{
    private readonly IEnumerable<IProgramInfoDataSourceService> _programInfoDataServices;

    public ProgramInfoDataService(IEnumerable<IProgramInfoDataSourceService> programInfoDataServices)
    {
        _programInfoDataServices = programInfoDataServices;
    }

    /// <inheritdoc/>
    public void FetchFallbackProperties(IProgramInfoData programInfoData) => GetProgramInfoDataService(programInfoData)?.FetchFallbackProperties(programInfoData);

    /// <inheritdoc/>
    public Task<bool> Modify(IProgramInfoData programInfoData, string? additionalArguments = null) => GetProgramInfoDataService(programInfoData)?.Modify(programInfoData, additionalArguments) ?? throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<bool> Uninstall(IProgramInfoData programInfoData, bool quiet = false) => GetProgramInfoDataService(programInfoData)?.Uninstall(programInfoData, quiet) ?? throw new NotImplementedException();

    /// <inheritdoc/>
    public void OpenLocation(IProgramInfoData programInfoData) => GetProgramInfoDataService(programInfoData)?.OpenLocation(programInfoData);

    /// <inheritdoc/>
    public void OpenLocationSource(IProgramInfoData programInfoData) => GetProgramInfoDataService(programInfoData)?.OpenLocationSource(programInfoData);

    /// <inheritdoc/>
    public void UpdateFromDifferent(IProgramInfoData programInfoDataToUpdate, IProgramInfoData programInfoDataToCopy) => GetProgramInfoDataService(programInfoDataToUpdate)?.UpdateFromDifferent(programInfoDataToUpdate, programInfoDataToCopy);

    private IProgramInfoDataSourceService? GetProgramInfoDataService(IProgramInfoData programInfoData) => _programInfoDataServices.FirstOrDefault(service => service.IsResponsible(programInfoData));
}
