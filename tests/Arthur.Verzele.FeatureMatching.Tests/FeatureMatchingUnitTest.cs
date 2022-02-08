using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using System.Reflection;
using System.Text.Json;

namespace Arthur.Verzele.FeatureMatching.Tests;

public class FeatureMatchingUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath, "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var objectImageData = await File.ReadAllBytesAsync(Path.Combine(executingPath, "Verzele-Arthur-object.jpg"));

        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenes(objectImageData, imageScenesData);
        
        Assert.Equal(
            "[{\"X\":2648,\"Y\":704},{\"X\":1570,\"Y\":821},{\"X\":1494,\"Y\":2220},{\"X\":2711,\"Y\":2243}]",
            JsonSerializer.Serialize(detectObjectInScenesResults[0].Points));
        
        Assert.Equal(
            "[{\"X\":2939,\"Y\":1074},{\"X\":2265,\"Y\":426},{\"X\":1324,\"Y\":701},{\"X\":2330,\"Y\":2209}]",
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