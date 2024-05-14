using Programs.Manager.Common.Win.Data;
using Programs.Manager.Common.Win.Service;
using Programs.Manager.Reader.Win.Service;
using Programs.Manager.Reader.Win.Test.Data.Faker;

namespace Programs.Manager.Reader.Win.Test.Service.ProgramInfo;

public sealed class ProgramInfoServiceTest
{
    [Fact]
    public void UpdateFromDifferent_WhenSecondNull_Success()
    {
        IRegistryService registryService = new RegistryService();
        IIconLoaderService iconLoaderService = new IconLoaderService();
        var service = new ProgramInfoService(registryService, iconLoaderService);
        var originalProgramInfo = new ProgramInfoData();
        var updateProgramInfo = new ProgramInfoDataFaker().Generate();

        service.UpdateFromDifferent(originalProgramInfo, updateProgramInfo);

        foreach (var property in typeof(ProgramInfoData).GetProperties())
        {
            var originalValue = property.GetValue(originalProgramInfo);

            var value = property.GetValue(originalProgramInfo);
            if (value is int intValue)
                Assert.NotEqual(-1, intValue);
            else if (value is long longValue)
                Assert.NotEqual(-1, longValue);
            else if (value is string stringValue)
                Assert.False(string.IsNullOrEmpty(stringValue));
            else
                Assert.NotNull(originalValue);
        }
    }

    [Fact]
    public void UpdateFromDifferent_WhenTheyDiffer_Success()
    {
        IRegistryService registryService = new RegistryService();
        IIconLoaderService iconLoaderService = new IconLoaderService();
        var service = new ProgramInfoService(registryService, iconLoaderService);
        var originalProgramInfo = new ProgramInfoDataFaker().Generate();
        originalProgramInfo.InstallLocation = null;
        originalProgramInfo.Language = null;
        originalProgramInfo.Comments = null;
        originalProgramInfo.UrlInfoAbout = null;
        originalProgramInfo.DisplayIcon = null;
        var updateProgramInfo = new ProgramInfoDataFaker().Generate();

        service.UpdateFromDifferent(originalProgramInfo, updateProgramInfo);

        foreach (var property in typeof(ProgramInfoData).GetProperties())
        {
            var originalValue = property.GetValue(originalProgramInfo);
            var updatedValue = property.GetValue(updateProgramInfo);
            Assert.Equal(updatedValue, originalValue);
        }
    }
}
