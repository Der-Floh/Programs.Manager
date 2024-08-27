using Microsoft.Extensions.DependencyInjection;
using ProgramInfos.Manager.Abstractions.Data;
using ProgramInfos.Manager.Abstractions.Repository;
using ProgramInfos.Manager.Abstractions.Service;
using ProgramInfos.Manager.Container.Repository;
using ProgramInfos.Manager.Container.Service.IconLoader;
using ProgramInfos.Manager.Container.Service.ProgramInfos;
using System.Reflection;

namespace ProgramInfos.Manager.Container;

/// <summary>
/// Service Collection Extensions for loading all plugin dependencies.
/// </summary>
public static class ServiceCollectionExtensions
{
    private const string _dllFilesFolder = "Plugins";
    private static readonly string[] _excludedAssemblies = [
        "System",
        "Microsoft",
        "mscorlib",
        "netstandard",
        "WindowsBase",
        "PresentationCore",
        "PresentationFramework",
        "MicroCom",
        "SkiaSharp",
        "Avalonia",
        "ProgramInfos.Manager.Abstractions",
        "ProgramInfos.Manager.Container"
    ];

    /// <summary>
    /// Registers all needed dependencies for all plugins.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the dependencies to.</param>
    /// <returns>An <see cref="IServiceCollection"/> with all dependencies registered.</returns>
    public static IServiceCollection AddProgramReaders(this IServiceCollection services)
    {
        var assemblies = GetAllAssemblies();
        RegisterType<IProgramInfoDataSourceRepository>(assemblies, services);
        RegisterType<IProgramInfoDataSourceService>(assemblies, services);
        RegisterServiceCollectionRegister(assemblies, services);

        services.AddSingleton<IIconLoaderService, IconLoaderService>();
        services.AddSingleton<IProgramInfoDataRepository, ProgramInfoDataRepository>();
        services.AddSingleton<IProgramInfoDataService, ProgramInfoDataService>();

        return services;
    }

    /// <summary>
    /// Gets all assemblies from the DLLs folder.
    /// </summary>
    /// <returns>An IEnumerable of <see cref="Assembly"/> with all found assemblies.</returns>
    private static IEnumerable<Assembly> GetAssembliesFromDllFiles()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _dllFilesFolder);
        if (!Directory.Exists(path))
            return [];

        var dllFiles = Directory.EnumerateFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _dllFilesFolder), "*.dll");
        return dllFiles.Select(Assembly.LoadFrom);
    }

    /// <summary>
    /// Gets all assemblies from the current assembly.
    /// </summary>
    /// <returns>An List of <see cref="Assembly"/> with all found assemblies.</returns>
    private static List<Assembly> GetAllAssemblies()
    {
        var currentAssembly = Assembly.GetEntryAssembly();

        var assemblies = new List<Assembly>();
        assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies());
        assemblies.AddRange(GetAssembliesFromDllFiles());

        var filteredAssemblies = assemblies.Where(assembly => !_excludedAssemblies.Any(exclusion => assembly?.FullName?.ToLower().StartsWith(exclusion, StringComparison.CurrentCultureIgnoreCase) ?? false)).ToList();

        var localAssemblyFiles = GetLocalAssemblyFiles();
        var filteredLocalAssemblyFiles = localAssemblyFiles.Where(assemblyPath => !_excludedAssemblies.Any(exclusion => Path.GetFileName(assemblyPath).StartsWith(exclusion, StringComparison.CurrentCultureIgnoreCase)));
        filteredLocalAssemblyFiles = localAssemblyFiles.Where(assemblyPath => !assemblies.Any(exclusion => assemblyPath.StartsWith(exclusion.Location, StringComparison.CurrentCultureIgnoreCase)));

        foreach (var assemblyPath in localAssemblyFiles)
        {
            var extension = Path.GetExtension(assemblyPath);
            Assembly? assembly = null;
            if (extension == ".dll")
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            else if (extension == ".pdb")
            {
                var dllBytes = File.ReadAllBytes(Path.ChangeExtension(assemblyPath, ".dll"));
                var pdbBytes = File.ReadAllBytes(assemblyPath);
                assembly = Assembly.Load(dllBytes, pdbBytes);
            }
            if (assembly is not null)
                filteredAssemblies.Add(assembly);
        }

        return filteredAssemblies;
    }

    /// <summary>
    /// Gets all local assembly files.
    /// </summary>
    /// <returns>An List of <see cref="string"/> with all found assembly file paths.</returns>
    private static List<string> GetLocalAssemblyFiles()
    {
        var files = new List<string>();
        var assemblyFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

#if DEBUG
        foreach (var assembly in assemblyFiles)
        {
            var pdbFile = Path.ChangeExtension(assembly, ".pdb");
            if (File.Exists(pdbFile))
                files.Add(pdbFile);
            else
                files.Add(assembly);
        }

        return files;
#else
        return assemblyFiles;
#endif
    }

    /// <summary>
    /// Registers all implementations of <see cref="IServiceCollectionRegister"/>.
    /// </summary>
    /// <param name="assemblies">An IEnumerable of <see cref="Assembly"/> with assemblies for which the implementations should be registered.</param>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the dependencies to.</param>
    private static void RegisterServiceCollectionRegister(IEnumerable<Assembly> assemblies, IServiceCollection services)
    {
        var implementations = GetImplementation<IServiceCollectionRegister>(assemblies);
        foreach (var implementation in implementations)
        {
            implementation.RegisterServices(services);
        }
    }

    /// <summary>
    /// Gets all implementations of an interface of type T.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <param name="assemblies">An IEnumerable of <see cref="Assembly"/> with assemblies in which the implementations should be searched.</param>
    /// <returns>An List of <see cref="TInterface"/> with all found implementations.</returns>
    private static List<TInterface> GetImplementation<TInterface>(IEnumerable<Assembly> assemblies)
    {
        var interfaceType = typeof(TInterface);
        var listImplementations = new List<TInterface>();
        foreach (var assembly in assemblies)
        {
            var implementations = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t));
            foreach (var implementation in implementations)
            {
                var instance = Activator.CreateInstance(implementation);
                if (instance is not null)
                {
                    var implementationInstance = (TInterface)instance;
                    listImplementations.Add(implementationInstance);
                }
            }
        }

        return listImplementations;
    }

    /// <summary>
    /// Registers all implementations of an interface of type T.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <param name="assemblies">An IEnumerable of <see cref="Assembly"/> with assemblies in which the implementations should be searched.</param>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the dependencies to.</param>
    private static void RegisterType<TInterface>(IEnumerable<Assembly> assemblies, IServiceCollection services)
    {
        var interfaceType = typeof(TInterface);

        foreach (var assembly in assemblies)
        {
            var implementations = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t));
            foreach (var implementation in implementations)
            {
                services.AddTransient(interfaceType, implementation);
            }
        }
    }
}
