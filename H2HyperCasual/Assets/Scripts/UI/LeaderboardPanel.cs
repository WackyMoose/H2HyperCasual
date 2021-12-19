using System.Linq;
using TMPro;
using UnityEngine;

public class LeaderboardPanel : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardListItemPrefab;
    [SerializeField] private Transform leaderboardListHolder;

    private async void Start()
    {
        var leaderboardPlayers = await APIManager.Instance.GetLeaderboardAsync(50);

        foreach (var leaderboardPlayer in leaderboardPlayers)
        {
            var leaderboardItemGO = Instantiate(leaderboardListItemPrefab, leaderboardListHolder);
            var leaderboardItem = leaderboardItemGO.GetComponent<LeaderboardListItem>();
            
            if (leaderboardItem != null)
            {
                var id = leaderboardPlayers.ToList().IndexOf(leaderboardPlayer);
                leaderboardItem.Setup(id, leaderboardPlayer);
            }
        }
    }
}
