using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TISA.Models;

namespace TISA.Services
{
    internal class QuestService : IQuestService
    {
        static QuestService()
        {
            //var quest1 = new Quest { Id = Guid.NewGuid(), Name = "Introductionary Quest", Description = "This is an easy introductionary quest", ExperienceReward = 100, GoldReward = 100 };
            //var quest2 = new Quest { Id = Guid.NewGuid(), Name = "Second quest", Description = "The quests are getting harder now, complete this very difficult quest", ExperienceReward = 200, GoldReward = 200, ComesAfterQuestId = quest1.Id };
            //_quests = new List<Quest> { quest1, quest2 };
        }

        private readonly IPlayerService _playerService;

        public QuestService(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public Task CompleteQuestAsync(Guid questId)
        {
            var player = _playerService.Player;
            return "https://localhost:7501/PlayerQuest/"
                .AppendPathSegment(player.Id.ToString())
                .PostJsonAsync(questId);
            //var completedQuests = GetCompletedQuestsForPlayer();
            //completedQuests.Add(questId);

            //// player management happening in quest service, bad practice!
            //var player = _playerService.Player;
            //var quest = await GetQuestByIdAsync(questId);
            //player.Experience += quest.ExperienceReward;
            //player.Level = (player.Experience / 100) + 1;
            //player.Gold += quest.GoldReward;
        }

        public async Task CreateAsync(Quest quest)
        {
            var response = await "https://localhost:7501/Quest/".PostJsonAsync(quest);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new InvalidOperationException("Something went wrong trying to create the quest");
            }
        }

        public async Task DeletByIdAsync(Guid questId)
        {
            var response = await "https://localhost:7501/Quest/".AppendPathSegment(questId).DeleteAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Something went wrong trying to delete the quest");
            }
        }

        public Task<IEnumerable<Quest>> GetAllQuestsAsync()
        {
            return "https://localhost:7501/Quest/".GetJsonAsync<IEnumerable<Quest>>();
        }

        public async Task<IEnumerable<Quest>> GetAvailableQuestsForPlayerAsync()
        {
            if (!_playerService.IsPlayerDefined)
            {
                throw new InvalidOperationException("Can't retrieve completed quests for player without a player being defined in the player service");
            }
            var player = _playerService.Player;
            return await "https://localhost:7501/PlayerQuest/"
                .AppendPathSegment(player.Id.ToString())
                .GetJsonAsync<List<Quest>>();
        }

        public Task<Quest> GetQuestByIdAsync(Guid questId)
        {
            return "https://localhost:7501/Quest/"
                .AppendPathSegment(questId.ToString())
                .GetJsonAsync<Quest>();
        }

        public async Task UpdateQuestByIdAsync(Guid questId, Quest newQuest)
        {
            var response = await "https://localhost:7501/Quest/"
                .AppendPathSegment(questId.ToString())
                .PutJsonAsync(newQuest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Something went wrong trying to update the quest");
            }
        }
    }
}
