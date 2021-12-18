using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class LeaderboardPanel : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardListItemPrefab;
    [SerializeField] private Transform leaderboardListHolder;

    private async void Start()
    {
        var leaderboardPlayers = await APIManager.Instance.GetLeaderboardAsync();

        foreach (var leaderboardPlayer in leaderboardPlayers)
        {
            var leaderboardItem = Instantiate(leaderboardListItemPrefab, leaderboardListHolder);
            var indexText = leaderboardItem.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
            indexText.text = $"#{(leaderboardPlayers.ToList().IndexOf(leaderboardPlayer)) + 1}";

            var playerNameText = leaderboardItem.transform.GetChild(1).GetComponentInChildren<TMP_Text>();
            playerNameText.text = leaderboardPlayer.playerName;

            var statsGameObject = leaderboardItem.transform.GetChild(2);
            var killsText = statsGameObject.GetChild(0).GetComponentInChildren<TMP_Text>();
            killsText.text = $"Kills: {leaderboardPlayer.kills}";

            var deathsText = statsGameObject.GetChild(1).GetComponentInChildren<TMP_Text>();
            deathsText.text = $"Deaths: {leaderboardPlayer.deaths}";

            var kdrText = statsGameObject.GetChild(2).GetComponentInChildren<TMP_Text>();
            kdrText.text = $"KDR: {leaderboardPlayer.kdr}";
        }
    }
}
