using Bogus;
using Programs.Manager.Reader.Win.Data;

namespace Programs.Manager.Reader.Win.Test.Data.Faker;

public sealed class ProgramRegInfoDataFaker : Faker<ProgramRegInfoData>
{
    public ProgramRegInfoDataFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid().ToString());
        RuleFor(x => x.AuthorizedCDFPrefix, f => f.Internet.Url());
        RuleFor(x => x.Comments, f => f.Lorem.Sentence());
        RuleFor(x => x.Contact, f => f.Phone.PhoneNumber());
        RuleFor(x => x.DisplayIcon, f => f.System.FilePath());
        RuleFor(x => x.DisplayName, f => f.Commerce.ProductName());
        var version = FakerHub.System.Version();
        RuleFor(x => x.DisplayVersion, f => version.ToString());
        var size = FakerHub.Random.Long(10, 20000000).ToString();
        RuleFor(x => x.EstimatedSize, f => size);
        RuleFor(x => x.HelpLink, f => f.Internet.Url());
        RuleFor(x => x.HelpTelephone, f => f.Phone.PhoneNumber());
        RuleFor(x => x.InstallDate, f => f.Date.Past().ToString());
        var installDir = FakerHub.System.DirectoryPath();
        RuleFor(x => x.InstallDir, f => installDir);
        RuleFor(x => x.InstallLocation, f => installDir);
        RuleFor(x => x.InstallSource, f => f.System.FilePath());
        RuleFor(x => x.Language, f => f.Random.RandomLocale());
        var versionMajor = version.Major.ToString();
        var versionMinor = version.Minor.ToString();
        RuleFor(x => x.MajorVersion, f => versionMajor);
        RuleFor(x => x.MinorVersion, f => versionMinor);
        RuleFor(x => x.ModifyPath, f => f.System.FilePath());
        RuleFor(x => x.NoModify, f => f.Random.Bool() ? "1" : "0");
        RuleFor(x => x.NoRemove, f => f.Random.Bool() ? "1" : "0");
        RuleFor(x => x.NoRepair, f => f.Random.Bool() ? "1" : "0");
        RuleFor(x => x.Publisher, f => f.Company.CompanyName());
        RuleFor(x => x.Readme, f => f.Internet.Url());
        RuleFor(x => x.Size, f => size);
        RuleFor(x => x.SystemComponent, f => f.Random.Bool() ? "1" : "0");
        RuleFor(x => x.QuietUninstallString, f => f.System.FilePath());
        RuleFor(x => x.UninstallString, f => f.System.FilePath());
        RuleFor(x => x.UrlInfoAbout, f => f.Internet.Url());
        RuleFor(x => x.UrlUpdateInfo, f => f.Internet.Url());
        RuleFor(x => x.Version, f => version.ToString());
        RuleFor(x => x.VersionMajor, f => versionMajor);
        RuleFor(x => x.VersionMinor, f => versionMinor);
        RuleFor(x => x.WindowsInstaller, f => f.Random.Bool() ? "1" : "0");
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
