using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TankGame.Models
{
    [Serializable]
    public class MatchData
    {
        public int MatchId { get; set; }
        public int MatchStatusId { get; set; }
        public int WinnerPlayerId { get; set; }
        public DateTime StartPlayTime { get; set; }
        public int PlayTime { get; set; }
        public List<Player> Players { get; set; }
        public List<MatchKill> MatchKills { get; set; }
        public Dictionary<ulong, int> ClientIdToPlayerIndex { get; set; }

        public MatchData()
        {
            StartPlayTime = DateTime.Now;

            Players = new List<Player>();
            MatchKills = new List<MatchKill>();
            ClientIdToPlayerIndex = new Dictionary<ulong, int>();
        }

        public bool AddPlayer(ulong clientId,Player player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
                ClientIdToPlayerIndex.Add(clientId, Players.IndexOf(player));
                return true;
            }

            return false;
        }

        public bool AddMatchKill(MatchKill matchKill)
        {
            if (!MatchKills.Contains(matchKill))
            {
                MatchKills.Add(matchKill);
                return true;
            }

            return false;
        }

        public bool AddKillToPlayer(ulong clientId)
        {
            Player player = Players.FirstOrDefault(p => p.id == ClientIdToPlayerIndex.GetValueOrDefault<ulong, int>(clientId));

            if (player != null)
            {
                player.kills++;

                UpdatePlayerKdr(clientId);

                return true;
            }

            return false;
        }

        public bool AddDeathToPlayer(ulong clientId)
        {
            Player player = Players.FirstOrDefault(p => p.id == ClientIdToPlayerIndex.GetValueOrDefault<ulong,int>(clientId));

            if (player != null)
            {
                player.deaths++;

                UpdatePlayerKdr(clientId);

                return true;
            }

            return false;
        }

        public bool UpdatePlayerKdr(ulong clientId)
        {
            Player player = Players.FirstOrDefault(p => p.id == ClientIdToPlayerIndex.GetValueOrDefault<ulong, int>(clientId));

            if (player != null)
            {
                player.kdr = (player.deaths != 0 ? player.kills / player.deaths : 0.0);

                return true;
            }

            return false;
        }

        public bool SetMatchWinner()
        {
            int maxKills = Players.Select(p => p.kills).Max();
            double maxKdr = Players.Select(p => p.kdr).Max();

            List<Player> PlayersSortedByKills = Players.OrderByDescending(p => p.kills).ToList();
            List<Player> PlayersSortedByKdr = PlayersSortedByKills.OrderByDescending(p => p.kdr).ToList();

            Player winner = PlayersSortedByKills.Intersect(PlayersSortedByKdr).First();

            WinnerPlayerId = winner.id;

            return true;
        }

        public bool SetPlayTime()
        {
            PlayTime = (int)(DateTime.Now - StartPlayTime).TotalSeconds;

            return true;
        }
    }
}