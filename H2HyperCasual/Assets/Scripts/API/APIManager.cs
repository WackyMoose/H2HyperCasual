using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class AuthRequest
{
    public string playerName;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public string accessToken;
    public Player player;
}

[System.Serializable]
public class RegisterResponse
{
    public Player player;
}

[System.Serializable]
public class LeaderboardResponse
{
    public LeaderboardPlayer[] players;
}

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    private PlayerManager _playerManager;
    private HttpRequest _httpRequest;

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
        _httpRequest = new HttpRequest();
    }

    private void Start()
    {
        _playerManager = PlayerManager.Instance;
    }

    public async Task<APIResponseWrapper<LoginResponse>> LoginAsync(string playerName, string password)
    {
        var loginRequest = new AuthRequest
        {
            playerName = playerName,
            password = password
        };

        return await _httpRequest.PostAsync<LoginResponse>("https://api.victorkrogh.dk/api/Auth/login", loginRequest);
    }

    public async Task<APIResponseWrapper<RegisterResponse>> RegisterAsync(string playerName, string password)
    {
        var registerRequest = new AuthRequest
        {
            playerName = playerName,
            password = password
        };

        return await _httpRequest.PostAsync<RegisterResponse>("https://api.victorkrogh.dk/api/Auth/register", registerRequest);
    }

    public async Task<IEnumerable<LeaderboardPlayer>> GetLeaderboardAsync(int top = 10)
    {
        // https://api.victorkrogh.dk/api/Leaderboard/get-leaderboard?top=10
        var leaderboardPlayers = await _httpRequest.GetAsync<IEnumerable<LeaderboardPlayer>>($"https://api.victorkrogh.dk/api/Leaderboard/get-leaderboard?top={top}");
        return leaderboardPlayers.Data;
    }
}