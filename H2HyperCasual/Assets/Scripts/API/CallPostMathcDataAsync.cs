using System.Collections;
using System.Collections.Generic;
using TankGame.Models;
using UnityEngine;


public class CallPostMathcDataAsync : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P is pressed!");

            MatchData matchData = CreateDummyMatchData();

            APIManager.Instance.PostMathcDataAsync(matchData);
        }
    }

    private MatchData CreateDummyMatchData()
    {

        var player1 = new Player
        {
            id = 10,
            playerName = "test1",
            kills = 0,
            deaths = 0
        };

        var player2 = new Player
        {
            id = 11,
            playerName = "testbruger",
            kills = 0,
            deaths = 0
        };

        var matchKill = new MatchKill
        {
            id = 3,
            matchId = 1,
            killerPlayerId = 1,
            killedPlayerId = 2,
            killDistance = 500
        };

        var matchData = new MatchData
        {
            MatchId = 0,
            MatchStatusId = 0,
            WinnerPlayerId = 0,
            PlayTime = 42,
            Players = { player1, player2 },
            MatchKills = { matchKill }
        };

        return matchData;
    }
}

//public int MatchId { get; set; }
//public int MatchStatusId { get; set; }
//public int WinnerPlayerId { get; set; }
//public DateTime StartPlayTime { get; set; }
//public int PlayTime { get; set; }
//public List<Player> Players { get; set; }
//public List<MatchKill> MatchKills { get; set; }
//public Dictionary<ulong, int> ClientIdToPlayerIndex { get; set; }

//public MatchData()
//{
//    StartPlayTime = DateTime.Now;

//    Players = new List<Player>();
//    MatchKills = new List<MatchKill>();
//    ClientIdToPlayerIndex = new Dictionary<ulong, int>();
//}
