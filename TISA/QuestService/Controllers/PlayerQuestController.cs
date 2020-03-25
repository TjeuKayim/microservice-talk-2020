using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestService.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestService.Controllers
{
    // handles player specific operations
    [Route("[controller]/{playerId}")]
    [ApiController]
    public class PlayerQuestController : ControllerBase
    {
        private readonly QuestDbContext _db;

        public PlayerQuestController(QuestDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quest>>> GetActiveQuests(Guid playerId) {
            var available = await _db.GetAvailableQuestsForPlayerIdAsync(playerId);
            return available;
        }

        [HttpPost]
        public async Task<IActionResult> CompleteQuest(Guid playerId, [FromBody] Guid questId) {
            var quest = new CompletedQuest { PlayerId = playerId, QuestId = questId };
            _db.CompletedQuests.Add(quest);
            await _db.SaveChangesAsync();
            return StatusCode(200);
        }
    }
}
