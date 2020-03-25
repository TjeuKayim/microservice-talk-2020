using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TISA.Models;

namespace TISA.Services
{
    internal class PlayerService : IPlayerService
    {
        //private static List<Player> _players = new List<Player>();
        public Player Player { get; set; }

        public bool IsPlayerDefined => Player != null;

        public async Task<Guid> CreatePlayerNameAsync(string playerName)
        {
            var response = await "https://localhost:7401/Player".PostJsonAsync(playerName);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new InvalidOperationException("Something went wrong trying to create the player");
            }

            var player = await $"https://localhost:7401{response.Headers.Location}".GetJsonAsync<Player>();
            return player.Id;
        }

        public async Task SetPlayerByPlayerIdAsync(Guid playerId)
        {
            var response = await "https://localhost:7401/Player/".AppendPathSegment(playerId.ToString()).GetJsonAsync<Player>();
            if (response == null)
            {
                throw new InvalidOperationException("Something went wrong trying to find the player");
            }
            Player = response;
        }
    }
}
