using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnPlayerLogin();
public delegate void OnPlayerLogout();

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private PlayerDataSO playerData;

    public event OnPlayerLogin OnPlayerLogin;
    public event OnPlayerLogout OnPlayerLogout;

    public bool IsAuthenticated
    {
        get
        {
            return playerData.player != null && !string.IsNullOrWhiteSpace(playerData.accessToken);
        }
    }

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

    public PlayerDataSO GetPlayerData()
    {
        return playerData;
    }

    public void PopulatePlayerData(string accessToken, Player player)
    {
        playerData.accessToken = accessToken;
        playerData.player = player;

        if(OnPlayerLogin != null)
        {
            OnPlayerLogin();
        }
    }

    public void ClearPlayerData()
    {
        playerData.accessToken = string.Empty;
        playerData.player = null;

        if (OnPlayerLogout != null)
        {
            OnPlayerLogout();
        }
    }
}
