using Microsoft.Extensions.DependencyInjection;
using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Events;
using ProgramInfos.Manager.Abstractions.Service;
using ProgramInfos.Manager.Reg.Repository.ProgramInfo;
using ProgramInfos.Manager.Reg.Service;
using ProgramInfos.Manager.Reg.Service.CRegistry;
using ProgramInfos.Manager.Reg.Service.IconLoader;
using System.Text.Json;

namespace ProgramInfos.Manager.Reg.Extensions;

/// <inheritdoc cref="IServiceCollectionRegister"/>
public sealed class ServiceCollectionExtensions : IServiceCollectionRegister
{
    /// <inheritdoc/>
    public void RegisterServices(IServiceCollection services, ImplementationLoadedEvent? implementationLoadedEvent = null)
    {
        AddSingleton<IProgramInfoDataRepository, ProgramInfoDataRepository>(services, implementationLoadedEvent);
        AddSingleton<IRegistryService, RegistryService>(services, implementationLoadedEvent);
        AddSingleton<IIconFinderService, IconFinderService>(services, implementationLoadedEvent);
        AddSingleton<IProgramInfoDataSourceService, ProgramInfoDataService>(services, implementationLoadedEvent);
        AddSingleton<IProgramInfoDataBackupSourceService, ProgramInfoDataBackupService>(services, implementationLoadedEvent);
        services.AddSingleton(new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
        });
    }

    /// <summary>
    /// Add singleton service and calls <see cref="ImplementationLoadedEvent"/>.
    /// </summary>
    /// <typeparam name="TService">The service type.</typeparam>
    /// <typeparam name="TImplementation">The implementation type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="implementationLoadedEvent">The implementation loaded event.</param>
    private void AddSingleton<TService, TImplementation>(IServiceCollection services, ImplementationLoadedEvent? implementationLoadedEvent = null) where TService : class where TImplementation : class, TService
    {
        services.AddSingleton<TService, TImplementation>();
        implementationLoadedEvent?.Invoke(this, new ImplementationLoadedEventArgs { LoadedInterface = typeof(TService), LoadedImplementation = typeof(TImplementation) });
    }
}
