using ProgramInfos.Manager.Reg.Service.EmbeddedResource;

namespace ProgramInfos.Manager.Reg.Test.Service.EmbeddedResource;

public class EmbeddedResourceServiceTest
{
    private readonly EmbeddedResourceService _service;
    private const string TestResourceNameExist = "languages.csv";
    private const string TestResourceNameNotExist = "thisfiledoesnotexist.lol";

    public EmbeddedResourceServiceTest() => _service = new EmbeddedResourceService();

    [Fact]
    public void GetResourcePath_WithExistingResource_ReturnsCorrectPath()
    {
        var path = _service.GetResourcePath(TestResourceNameExist);
        Assert.Equal(_service.GetResourcePath(TestResourceNameExist), path);
    }

    [Fact]
    public void GetResourcePath_WithNonExistingResource_ThrowsFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => _service.GetResourcePath(TestResourceNameNotExist));
    }

    [Fact]
    public void GetResourceStream_WithExistingResource_ReturnsStream()
    {
        using var stream = _service.GetResourceStream(TestResourceNameExist);
        Assert.NotNull(stream);
    }

    [Fact]
    public void GetResourceStream_WithNonExistingResource_ThrowsFileNotFoundException()
    {
        Assert.Throws<FileNotFoundException>(() => _service.GetResourceStream(TestResourceNameNotExist));
    }
}
