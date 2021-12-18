using System.Collections;
using System.Collections.Generic;
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

    public bool AddMatchKill(MatchKill matchKill)
    {
        if (!MatchKills.Contains(matchKill))
        {
            MatchKills.Add(matchKill);
            return true;
        }

        return false;
    }

    public bool SetMatchWinner()
    {
        return true;
    }
}
