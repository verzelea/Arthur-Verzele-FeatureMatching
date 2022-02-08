using System.Reflection;
using System.Text.Json;

namespace Arthur.Verzele.FeatureMatching.Console;

class Program
{
    static async Task Main(string[] args)
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath, args[1])))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var objectImageData = await File.ReadAllBytesAsync(Path.Combine(executingPath, args[0]));
        
        var detectObjectInScenesResults = await new ObjectDetection().DetectObjectInScenes(objectImageData, imageScenesData);

        foreach (var objectDetectionResult in detectObjectInScenesResults)
        {
            System.Console.WriteLine($"Points:{JsonSerializer.Serialize(objectDetectionResult.Points)}");
        }
    }
    
    private static string GetExecutingPath()
    {
        var executingAssemblyPath =
            Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath ?? string.Empty;
    }
}


