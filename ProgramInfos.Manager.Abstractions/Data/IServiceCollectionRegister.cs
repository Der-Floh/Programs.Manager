using Microsoft.Extensions.DependencyInjection;

namespace ProgramInfos.Manager.Abstractions.Data;

/// <summary>
/// Interface to register services for plugins.
/// </summary>
public interface IServiceCollectionRegister
{
    /// <summary>
    /// Registers services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    void RegisterServices(IServiceCollection services);
}
