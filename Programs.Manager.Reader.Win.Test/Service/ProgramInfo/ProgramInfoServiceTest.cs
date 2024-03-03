using Moq;
using Programs.Manager.Common.Win.Data;
using Programs.Manager.Common.Win.Service.RegJump;
using Programs.Manager.Reader.Win.Service;
using Programs.Manager.Reader.Win.Test.Data.Faker;
using System.Reflection;

namespace Programs.Manager.Reader.Win.Test.Service.ProgramInfo;

public sealed class ProgramInfoServiceTest
{
    [Fact]
    public void UpdateFromDifferent_WhenTheyDiffer_Success()
    {
        var mockRegJumpService = new Mock<IRegJumpService>();
        var service = new ProgramInfoService(mockRegJumpService.Object);
        var originalProgramInfo = new ProgramInfoDataFaker().Generate();
        originalProgramInfo.InstallLocation = null;
        originalProgramInfo.Language = null;
        originalProgramInfo.Comments = null;
        originalProgramInfo.UrlInfoAbout = null;
        originalProgramInfo.DisplayIcon = null;
        var updateProgramInfo = new ProgramInfoDataFaker().Generate();

        service.UpdateFromDifferent(originalProgramInfo, updateProgramInfo);

        foreach (PropertyInfo property in typeof(ProgramInfoData).GetProperties())
        {
            var originalValue = property.GetValue(originalProgramInfo);
            var updatedValue = property.GetValue(updateProgramInfo);
            Assert.Equal(updatedValue, originalValue);
        }
    }

    [Fact]
    public void UpdateFromDifferent_WhenSecondNull_Success()
    {
        var mockRegJumpService = new Mock<IRegJumpService>();
        var service = new ProgramInfoService(mockRegJumpService.Object);
        var originalProgramInfo = new ProgramInfoDataFaker().Generate();
        var updateProgramInfo = new ProgramInfoData(service);

        service.UpdateFromDifferent(originalProgramInfo, updateProgramInfo);

        foreach (PropertyInfo property in typeof(ProgramInfoData).GetProperties())
        {
            var originalValue = property.GetValue(originalProgramInfo);
            Assert.NotNull(originalValue);
        }
    }
}
