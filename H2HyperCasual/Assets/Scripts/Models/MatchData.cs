using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TankGame.Models
{
    public class MatchData
    {
        public int MatchStatus { get; set; }
        public int WinnerPlayerId { get; set; }
        public int StartPlayTime { get; set; }
        public int PlayTime { get; set; }
        public List<Player> Players { get; set; }
        public List<MatchKill> MatchKills { get; set; }

        public MatchData()
        {
            StartPlayTime = DateTime.Now.Second;

            Players = new List<Player>();
            MatchKills = new List<MatchKill>();
        }

        public bool AddPlayer(Player player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
                return true;
            }

            return false;
        }

        public bool AddKillToMatchKill(MatchKill matchKill)
        {
            if (!MatchKills.Contains(matchKill))
            {
                MatchKills.Add(matchKill);
                return true;
            }

            return false;
        }

        public bool AddKillToPlayer(int playerId)
        {
            Player player = Players.First(p => p.id == playerId);
            
            if (player != null)
            {
                player.kills++;

                UpdatePlayerKdr(playerId);

                return true;
            }

            return false;
        }

        public bool AddDeathToPlayer(int playerId)
        {
            Player player = Players.First(p => p.id == playerId);

            if (player != null)
            {
                player.deaths++;

                UpdatePlayerKdr(playerId);

                return true;
            }

            return false;
        }

        public bool UpdatePlayerKdr(int playerId)
        {
            Player player = Players.First(p => p.id == playerId);

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
            List<Player> PlayersSortedByKdr = Players.OrderByDescending(p => p.kdr).ToList();

            Player winner = PlayersSortedByKills.Intersect(PlayersSortedByKdr).First();

            WinnerPlayerId = winner.id;

            return true;
        }

        public bool SetPlayTime()
        {
            PlayTime = DateTime.Now.Second - StartPlayTime;

            return true;
        }
    }
}