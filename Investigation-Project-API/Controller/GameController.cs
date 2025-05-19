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
    public IActionResult Post([FromBody] Game game)
    {
        if (game == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(game.Title) ||
        string.IsNullOrWhiteSpace(game.Platform) ||
        string.IsNullOrWhiteSpace(game.Genre))
        {
            return BadRequest("Title, Platform, and Genre are required.");
        }

        var createdGame = new GameData().InsertGame(game.Title, game.Platform, game.HoursPlayed, game.IsCompleted, game.Genre);
        if (createdGame == null)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Get), new { id = createdGame.Id }, createdGame);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Game game)
    {
        if (game == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(game.Title) ||
        string.IsNullOrWhiteSpace(game.Platform) ||
        string.IsNullOrWhiteSpace(game.Genre))
        {
            return BadRequest("Title, Platform, and Genre are required.");
        }

        var updatedGame = new GameData().UpdateGame(id, game.Title, game.Platform, game.HoursPlayed, game.IsCompleted, game.Genre);
        if (updatedGame == null)
        {
            return NotFound();
        }
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