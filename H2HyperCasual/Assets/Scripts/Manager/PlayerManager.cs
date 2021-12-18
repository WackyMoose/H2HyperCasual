using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    public string AccessToken;
    public int Id;
    public string Name;
    public int Kills;
    public int Deaths;
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private PlayerData _playerData;

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
}
