using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchData : MonoBehaviour
{
    public int MatchStatus { get; set; }
    public int WinnerPlayerId { get; set; }
    public int PlayTime { get; set; }
    public List<Player> Players { get; set; }
    public List<MatchKill> MatchKills { get; set; }

    public bool AddPlayer(Player player)
    {
        if(!Players.Contains(player))
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

    public bool AddKillToPlayer(Player player)
    {
        if (!Players.Contains(player))
        {
            player.kills++;
            return true;
        }

        return false;
    }

    public bool AddDeathToPlayer(Player player)
    {
        if (!Players.Contains(player))
        {
            player.deaths++;
            return true;
        }

        return false;
    }

    public bool UpdatePlayerKdr(Player player)
    {
        if (!Players.Contains(player))
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

        Player winner = Players.Find(p => p.kills == maxKills && p.kdr == maxKdr);

        WinnerPlayerId = winner.id;

        return true;
    }
}
