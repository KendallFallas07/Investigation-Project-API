using Microsoft.AspNetCore.Mvc;
using Model;

namespace Controller;

[ApiController]
[Route("api/games")]
public class GameController : ControllerBase
{

    [HttpGet]
    public IEnumerable<Game> Get()
    {
        return new GameData().GetAllGames();
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var game = new GameData().GetGameById(id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] string title,
        [FromForm] string platform,
        [FromForm] int hoursPlayed,
        [FromForm] bool isCompleted,
        [FromForm] string genre,
        IFormFile image)

    {

        if (!new GameData().IsTitleAvailable(title))
        {
            return BadRequest("Title already exists.");
        }

        string ImageUrl = string.Empty;
        if (image != null && image.Length > 0)
        {
            // Generate a unique file name to avoid conflicts
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
            var fileName = timestamp + "-" + Path.GetFileName(image.FileName);

            // Define the path to save the image
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages", fileName);

            var directoryPath = Path.GetDirectoryName(filePath);
            // Verifie if the directory exists, if not, create it
            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Relative Url to access the image
            ImageUrl = "/UploadedImages/" + fileName;
        }

        var insertedGame = new GameData().InsertGame(title, ImageUrl, platform, hoursPlayed, isCompleted, genre);
        return Ok(insertedGame);

    }

    [HttpPut]
    public async Task<IActionResult> Put([FromForm] int id,
        [FromForm] string title,
        [FromForm] string platform,
        [FromForm] int hoursPlayed,
        [FromForm] bool isCompleted,
        [FromForm] string genre,
        IFormFile? image)
    {

        if (!new GameData().IsTitleAvailable(title, id))
        {
            return BadRequest("Title already exists.");
        }

        string ImageUrl = string.Empty;
        var game = new GameData().GetGameById(id);

        if (image != null && image.Length > 0 && !string.IsNullOrEmpty(game?.ImageUrl))
        {
            // Generate a unique file name to avoid conflicts
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
            var fileName = timestamp + "-" + Path.GetFileName(image.FileName);

            // Delete the old image if it exists
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), game.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            // Define the path to save the image
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages", fileName);
            var directoryPath = Path.GetDirectoryName(filePath);
            // Verify if the directory exists, if not, create it
            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the image to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Relative Url to access the image
            ImageUrl = "/UploadedImages/" + fileName;

        }
        
        var newImageUrl = string.IsNullOrEmpty(ImageUrl) ? game?.ImageUrl : ImageUrl;

        var updatedGame = new GameData().UpdateGame(id, title, newImageUrl!, platform, hoursPlayed, isCompleted, genre);
        return Ok(updatedGame);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var game = new GameData().GetGameById(id);
        if (game == null)
        {
            return NotFound();
        }

        var isDeleted = new GameData().DeleteGame(id);
        if (!isDeleted)
        {
            return BadRequest();
        }
        return Ok(new { message = "Game deleted successfully." });
    }
}