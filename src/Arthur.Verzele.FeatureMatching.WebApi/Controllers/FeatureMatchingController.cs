using Microsoft.AspNetCore.Mvc;

namespace Arthur.Verzele.FeatureMatching.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureMatchingController : ControllerBase
{
    private readonly ObjectDetection _objectDetection;
    
    public FeatureMatchingController(ObjectDetection objectDetection)
    {
        _objectDetection = objectDetection;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
    {
        if(files.Count != 2)
            return BadRequest();

        using var objectSourceStream = files[0].OpenReadStream();
        using var objectMemoryStream = new MemoryStream();
        objectSourceStream.CopyTo(objectMemoryStream);
        var imageObjectData = objectMemoryStream.ToArray();

        using var sceneSourceStream = files[1].OpenReadStream();
        using var sceneMemoryStream = new MemoryStream();
        sceneSourceStream.CopyTo(sceneMemoryStream);
        var imageSceneData = sceneMemoryStream.ToArray();

        // Your implementation code
        var detectObjectInScenesResults = 
            await _objectDetection.DetectObjectInScenes(imageObjectData, new List<byte[]>{imageSceneData});


        // La méthode ci-dessous permet de retourner une image depuis un tableau de bytes
        var imageData = detectObjectInScenesResults[0].ImageData;
        return File(imageData, "image/png");
    }
}
