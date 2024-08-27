using System.Reflection;

namespace ProgramInfos.Manager.Reg.Service.EmbeddedResource;

///<inheritdoc cref="IEmbeddedResourceService"/>
public sealed class EmbeddedResourceService : IEmbeddedResourceService
{
    private readonly string _resourcesPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddedResourceService"/> class.
    /// </summary>
    public EmbeddedResourceService()
    {
        _resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
    }

    ///<inheritdoc/>
    public string GetResourcePath(string name)
    {
        if (!ResourceExists(name))
        {
            if (!CopyResource(name))
                throw new FileNotFoundException(name);
        }

        return Path.Combine(_resourcesPath, name);
    }

    ///<inheritdoc/>
    public Stream GetResourceStream(string name) => new FileStream(GetResourcePath(name), FileMode.Open, FileAccess.Read);

    ///<inheritdoc/>
    public bool ResourceExists(string name) => File.Exists(Path.Combine(_resourcesPath, name));

    ///<inheritdoc/>
    public bool CopyResource(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var names = assembly.GetManifestResourceNames();
        var manifestResourceName = names.FirstOrDefault(file => file.Contains(name));
        if (string.IsNullOrEmpty(manifestResourceName))
            return false;

        using var resourceStream = assembly.GetManifestResourceStream(manifestResourceName);
        if (resourceStream is null)
            return false;

        using var fileStream = new FileStream(Path.Combine(_resourcesPath, name), FileMode.Create, FileAccess.Write);
        resourceStream.CopyTo(fileStream);

        return true;
    }
}
