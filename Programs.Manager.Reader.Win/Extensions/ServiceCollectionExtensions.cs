using Microsoft.Extensions.DependencyInjection;
using Programs.Manager.Reader.Win.Repository.ProgramInfo;
using Programs.Manager.Reader.Win.Repository.ProgramRegInfo;
using Programs.Manager.Reader.Win.Service.EmbeddedResource;
using Programs.Manager.Reader.Win.Service.IconLoader;
using Programs.Manager.Reader.Win.Service.ProgramInfo;
using Programs.Manager.Reader.Win.Service.ProgramRegInfo;
using Programs.Manager.Reader.Win.Service.RegJump;
using Programs.Manager.Reader.Win.Service.WindowsLanguage;

namespace Programs.Manager.Reader.Win.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds services related to Windows program information and Windows language data to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <remarks>
    /// This method registers the following services:<br/>
    /// - <see cref="IProgramInfoRepository"/> implemented by <see cref="ProgramInfoRepository"/>.<br/>
    /// - <see cref="IProgramInfoService"/> implemented by <see cref="ProgramInfoService"/>.<br/>
    /// - <see cref="IProgramRegInfoService"/> implemented by <see cref="ProgramRegInfoService"/>.<br/>
    /// - <see cref="IProgramRegInfoRepository"/> implemented by <see cref="ProgramRegInfoRepository"/>.<br/>
    /// - <see cref="IWindowsLanguageService"/> implemented by <see cref="WindowsLanguageService"/>.<br/>
    /// - <see cref="IRegJumpService"/> implemented by <see cref="RegJumpService"/>.<br/>
    /// - <see cref="IEmbeddedResourceService"/> implemented by <see cref="EmbeddedResourceService"/>.<br/>
    /// - <see cref="IIconLoaderService"/> implemented by <see cref="IconLoaderService"/>.<br"/>
    /// These services are essential for managing and accessing information about installed Windows programs and language settings.
    /// </remarks>
    public static IServiceCollection AddWindowsProgramsReader(this IServiceCollection services)
    {
        services.AddSingleton<IProgramInfoRepository, ProgramInfoRepository>();
        services.AddSingleton<IProgramInfoService, ProgramInfoService>();
        services.AddSingleton<IProgramRegInfoService, ProgramRegInfoService>();
        services.AddSingleton<IProgramRegInfoRepository, ProgramRegInfoRepository>();
        services.AddSingleton<IWindowsLanguageService, WindowsLanguageService>();
        services.AddSingleton<IRegJumpService, RegJumpService>();
        services.AddSingleton<IEmbeddedResourceService, EmbeddedResourceService>();
        services.AddSingleton<IIconLoaderService, IconLoaderService>();
        return services;
    }
}
