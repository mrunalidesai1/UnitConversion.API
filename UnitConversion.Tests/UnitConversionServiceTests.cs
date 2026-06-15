using UnitConversion.API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System;
using Xunit;

namespace UnitConversion.Tests;

public class UnitConversionServiceTests
{
    private readonly UnitConversionService _service;

    public UnitConversionServiceTests()
    {
        // Provide a test IWebHostEnvironment with a temp content root so unit file operations do not interfere with developer files.
        var tempDir = Path.Combine(Path.GetTempPath(), "UnitConversionTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        var env = new TestEnvironment { ContentRootPath = tempDir, WebRootPath = tempDir };
        // set file providers to avoid nullability issues
        env.ContentRootFileProvider = new PhysicalFileProvider(tempDir);
        env.WebRootFileProvider = new PhysicalFileProvider(tempDir);
        _service = new UnitConversionService(env);
    }

    private class TestEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = string.Empty;
        public IFileProvider? ContentRootFileProvider { get; set; }
        public string ContentRootPath { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = "Development";
        public IFileProvider? WebRootFileProvider { get; set; }
        public string WebRootPath { get; set; } = string.Empty;
    }

    [Fact]
    public void MetersToFeet()
    {
        var res = _service.Convert("meters", "feet", 1);
        Assert.NotNull(res);
        Assert.Equal("length", res!.Category);
        Assert.Equal(3.28084, res.Output, 5);
    }

    [Fact]
    public void KgToLb()
    {
        var res = _service.Convert("kg", "lb", 1);
        Assert.NotNull(res);
        Assert.Equal("weight", res!.Category);
        Assert.Equal(2.20462262, res.Output, 6);
    }

    [Fact]
    public void CtoF()
    {
        var res = _service.Convert("c", "f", 100);
        Assert.NotNull(res);
        Assert.Equal("temperature", res!.Category);
        Assert.Equal(212.0, res.Output, 4);
    }
}
