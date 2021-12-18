using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Data/new Player Data", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    public string accessToken;
    public Player player;
}
