using Programs.Manager.Reader.Win.Data;
using Programs.Manager.Reader.Win.Service.IconLoader;
using System.Drawing;

namespace Programs.Manager.Reader.Win.Service.ProgramRegInfo;

///<inheritdoc cref="IProgramRegInfoService"/>
public sealed class ProgramRegInfoService : IProgramRegInfoService
{
    private readonly IIconLoaderService _iconLoaderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramRegInfoService"/> class.
    /// </summary>
    /// <param name="iconLoaderService">The <see cref="IIconLoaderService"/> object to use for icon retrieving operations.</param>
    public ProgramRegInfoService(IIconLoaderService iconLoaderService)
    {
        _iconLoaderService = iconLoaderService;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramRegInfoService"/> class.
    /// </summary>
    public ProgramRegInfoService()
    {
        _iconLoaderService = new IconLoaderService();
    }

    public void FetchFallbackValues(ProgramRegInfoData programRegInfoData)
    {
        if (string.IsNullOrEmpty(programRegInfoData.InstallLocation) && !string.IsNullOrEmpty(programRegInfoData.DisplayIcon))
        {
            var iconPath = _iconLoaderService.RemoveIconIndex(programRegInfoData.DisplayIcon);
            iconPath = iconPath.Trim('"');
            var extension = Path.GetExtension(programRegInfoData.DisplayIcon).ToLower();
            if (extension.Contains(".exe"))
                programRegInfoData.InstallLocation = Path.GetDirectoryName(iconPath) ?? string.Empty;
        }

        if ((string.IsNullOrEmpty(programRegInfoData.EstimatedSize) || programRegInfoData.EstimatedSize == "0") && !string.IsNullOrEmpty(programRegInfoData.InstallLocation))
        {
            try
            {
                var directoryInfo = new DirectoryInfo(programRegInfoData.InstallLocation);
                long totalSize = 0;
                FileInfo[] fileInfos = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
                foreach (FileInfo fileInfo in fileInfos)
                    totalSize += fileInfo.Length;
                programRegInfoData.EstimatedSize = (totalSize / 1024).ToString();
            }
            catch { }
        }
    }

    public Bitmap? GetIcon(ProgramRegInfoData programRegInfoData)
    {
        try
        {
            return _iconLoaderService.GetIcon(programRegInfoData);
        }
        catch { return null; }
    }
}
