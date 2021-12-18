using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

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

    public async Task Login(string playerName, string password)
    {
        var loginRequest = new AuthRequest
        {
            playerName = playerName,
            password = password
        };

        var loginResponse = await _httpRequest.PostAsync<LoginResponse>("https://api.victorkrogh.dk/api/Auth/login", loginRequest);
        Debug.Log(loginResponse.accessToken);
    }

    public async Task Register(string playerName, string password)
    {
        var registerRequest = new AuthRequest
        {
            playerName = playerName,
            password = password
        };

        var registerResponse = await _httpRequest.PostAsync<RegisterResponse>("https://api.victorkrogh.dk/api/Auth/register", registerRequest);
        Debug.Log(registerResponse.player.playerName);
    }
}