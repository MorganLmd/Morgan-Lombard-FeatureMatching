using System.Collections.ObjectModel;
using System.Text.Json;
using Morgan.Lombard.FeatureMatching;

class MyConsoleProgram
{
    static async Task Main(string[] args)
    {
        var imageToSearchPath = args[0];
        var objectImageData = await File.ReadAllBytesAsync(imageToSearchPath);
        var sceneFolderPath = args[1];
        var filesFromDisk = Directory.GetFiles(sceneFolderPath);
        
        var imagesSceneData = new List<byte[]>();
        foreach (var file in filesFromDisk)
        {
            imagesSceneData.Add(await File.ReadAllBytesAsync(file));
        }

        var objectDetection = new ObjectDetection();
        var objectDetectionResults = await objectDetection.DetectObjectInScenes(objectImageData, imagesSceneData);

        foreach (var objectDetectionResult in objectDetectionResults)
        {
            System.Console.WriteLine($"Points: {JsonSerializer.Serialize(objectDetectionResult.Points)}");
        }
    }
}