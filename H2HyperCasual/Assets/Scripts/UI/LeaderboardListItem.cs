using TMPro;
using UnityEngine;

public class LeaderboardListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text deathsText;
    [SerializeField] private TMP_Text kdrText;

    public void Setup(int id, LeaderboardPlayer leaderboardPlayer)
    {
        idText.text = $"#{id}";
        playerNameText.text = leaderboardPlayer.playerName;
        killsText.text = leaderboardPlayer.kills.ToString();
        deathsText.text = leaderboardPlayer.deaths.ToString();
        kdrText.text = leaderboardPlayer.kdr.ToString();
    }
}
