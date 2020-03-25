using Microsoft.AspNetCore.Mvc;
using PlayerService.Database;
using System;
using System.Threading.Tasks;

namespace PlayerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerDbContext _db;

        public PlayerController(PlayerDbContext db)
        {
            _db = db;
        }

        // This should return a Player object by its player id, or NotFound when null
        [HttpGet("{playerId}")]
        public async Task<ActionResult<Player>> GetPlayer(Guid playerId) {
            var player = await _db.Players.FindAsync(playerId);
            if (player == null)
            {
                return NotFound();
            }
            return player;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPlayer([FromBody] string playerName)
        {
            var player = new Player { Name = playerName };
            _db.Players.Add(player);
            await _db.SaveChangesAsync();
            // creation logic
            return Created($"/Player/{player.Id}", player);
        }
    }
}
