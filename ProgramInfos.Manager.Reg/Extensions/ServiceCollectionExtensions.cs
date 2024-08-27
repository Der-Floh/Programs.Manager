using Microsoft.Extensions.DependencyInjection;
using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Service;
using ProgramInfos.Manager.Reg.Repository.ProgramInfo;
using ProgramInfos.Manager.Reg.Service;
using ProgramInfos.Manager.Reg.Service.CRegistry;
using ProgramInfos.Manager.Reg.Service.EmbeddedResource;
using ProgramInfos.Manager.Reg.Service.IconLoader;

namespace ProgramInfos.Manager.Reg.Extensions;

/// <inheritdoc cref="IServiceCollectionRegister"/>
public class ServiceCollectionExtensions : IServiceCollectionRegister
{
    /// <inheritdoc/>
    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IProgramInfoDataRepository, ProgramInfoDataRepository>();
        services.AddSingleton<IRegistryService, RegistryService>();
        services.AddSingleton<IEmbeddedResourceService, EmbeddedResourceService>();
        services.AddSingleton<IIconFinderService, IconFinderService>();
        services.AddSingleton<IProgramInfoDataSourceService, ProgramInfoDataService>();
    }
}
