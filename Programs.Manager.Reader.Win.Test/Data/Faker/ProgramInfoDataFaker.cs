using Bogus;
using Programs.Manager.Common.Win.Data;
using System.Drawing;

namespace Programs.Manager.Reader.Win.Test.Data.Faker;

public sealed class ProgramInfoDataFaker : Faker<ProgramInfoData>
{
    public ProgramInfoDataFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid().ToString());
        RuleFor(x => x.Comments, f => f.Lorem.Sentence());
        RuleFor(x => x.Contact, f => f.Phone.PhoneNumber());
        var displayIcon = new Bitmap(32, 32);
        using var gfx = Graphics.FromImage(displayIcon);
        gfx.Clear(Color.Green);
        RuleFor(x => x.DisplayIcon, displayIcon);
        RuleFor(x => x.DisplayName, f => f.Commerce.ProductName());
        var version = FakerHub.System.Version();
        RuleFor(x => x.DisplayVersion, f => version.ToString());
        RuleFor(x => x.EstimatedSize, f => f.Random.Long(10, 20000000));
        RuleFor(x => x.HelpLink, f => f.Internet.Url());
        RuleFor(x => x.HelpTelephone, f => f.Phone.PhoneNumber());
        RuleFor(x => x.InstallDate, f => f.Date.Past().ToString());
        RuleFor(x => x.InstallLocation, f => f.System.DirectoryPath());
        RuleFor(x => x.InstallSource, f => f.System.FilePath());
        RuleFor(x => x.Language, f => f.Random.RandomLocale());
        RuleFor(x => x.ModifyPath, f => f.System.FilePath());
        RuleFor(x => x.NoModify, f => f.Random.Bool());
        RuleFor(x => x.NoRemove, f => f.Random.Bool());
        RuleFor(x => x.NoRepair, f => f.Random.Bool());
        RuleFor(x => x.Publisher, f => f.Company.CompanyName());
        RuleFor(x => x.Readme, f => f.Internet.Url());
        RuleFor(x => x.SystemComponent, f => f.Random.Bool());
        RuleFor(x => x.QuietUninstallString, f => f.System.FilePath());
        RuleFor(x => x.UninstallString, f => f.System.FilePath());
        RuleFor(x => x.UrlInfoAbout, f => f.Internet.Url());
        RuleFor(x => x.UrlUpdateInfo, f => f.Internet.Url());
        RuleFor(x => x.VersionMajor, f => version.Major);
        RuleFor(x => x.VersionMinor, f => version.Minor);
        RuleFor(x => x.WindowsInstaller, f => f.Random.Bool());
        RuleFor(x => x.RegKey, f =>
        {
            var rootKeys = new[] { "HKEY_LOCAL_MACHINE", "HKEY_CURRENT_USER", "HKEY_CLASSES_ROOT", "HKEY_USERS", "HKEY_CURRENT_CONFIG" };
            var root = f.PickRandom(rootKeys);
            var companyName = f.Company.CompanyName().Replace(" ", "").Replace(",", "").Replace(".", "");
            var softwareName = f.Commerce.ProductName().Replace(" ", "").Replace(",", "").Replace(".", "");
            var version = f.System.Version().ToString();

            return @$"{root}\Software\{companyName}\{softwareName}\{version}";
        });
    }
}
