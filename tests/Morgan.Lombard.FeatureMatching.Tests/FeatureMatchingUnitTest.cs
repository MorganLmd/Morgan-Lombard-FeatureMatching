using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Morgan.Lombard.FeatureMatching.Tests;

public class FeatureMatchingUnitTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public FeatureMatchingUnitTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in
                 Directory.EnumerateFiles(Path.Combine(executingPath, "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var objectImageData = await
            File.ReadAllBytesAsync(Path.Combine(executingPath, "Morgan-Lombard-object.jpg"));
        var detectObjectInScenesResults =
            await new ObjectDetection().DetectObjectInScenes(objectImageData, imageScenesData);
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(detectObjectInScenesResults[0].Points));
        Assert.Equal("[{\"X\":1637,\"Y\":343},{\"X\":321,\"Y\":1970},{\"X\":1401,\"Y\":2779},{\"X\":2652,\"Y\":1378}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[0].Points));
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(detectObjectInScenesResults[1].Points));
        Assert.Equal("[{\"X\":1790,\"Y\":528},{\"X\":318,\"Y\":1479},{\"X\":1024,\"Y\":2505},{\"X\":2439,\"Y\":1685}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[1].Points));
    }

    private static string GetExecutingPath()
    {
        var executingAssemblyPath =
            Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}