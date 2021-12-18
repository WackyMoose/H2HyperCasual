using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private PlayerDataSO playerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void PopulatePlayerData(string accessToken, Player player)
    {
        playerData.accessToken = accessToken;
        playerData.player = player;
    }
}
