using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestService.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly QuestDbContext _db;

        public QuestController(QuestDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quest>>> GetQuests() {
            return await _db.Quests.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuest(Quest quest) {
            _db.Quests.Add(quest);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuestById), new { questId = quest.Id }, quest);
        }

        [HttpGet("{questId}")]
        public async Task<ActionResult<Quest>> GetQuestById(Guid questId) {
            return await _db.Quests.FindAsync(questId);
        }

        [HttpDelete("{questId}")]
        public async Task<IActionResult> DeleteQuest(Guid questId) {
            var quest = await _db.Quests.FindAsync(questId);
            _db.Remove(quest);
            await _db.SaveChangesAsync();
            return StatusCode(200);
        }

        [HttpPut("{questId}")]
        public async Task<IActionResult> UpdateQuest(Guid questId, Quest quest) {
            var original = await _db.Quests.FindAsync(questId);
            original.Name = quest.Description;
            original.Description = quest.Description;
            original.GoldReward = quest.GoldReward;
            original.ExperienceReward = quest.ExperienceReward;
            original.ComesAfterQuestId = quest.ComesAfterQuestId;
            return StatusCode(200);
        }

    }
}
