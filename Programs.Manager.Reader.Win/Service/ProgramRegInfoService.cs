using Programs.Manager.Common.Data;
using Programs.Manager.Common.Service.IconLoader;
using Programs.Manager.Common.Service.ProgramRegInfo;
using System.Drawing;

namespace Programs.Manager.Reader.Win.Service;

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

    public Bitmap? GetIcon(ProgramRegInfoData programRegInfoData)
    {
        try
        {
            return _iconLoaderService.GetIcon(programRegInfoData);
        }
        catch { return null; }
    }
}
