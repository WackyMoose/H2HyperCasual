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
    public static PlayerManager instance;

    [SerializeField] private PlayerData _playerData;

    private void Awake()
    {
        instance = this;
    }
}
