using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LoginRequest
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

public class Player
{
    public int id;
    public string playerName;
    public int kills;
    public int deaths;
}


public class APIManager : MonoBehaviour
{
    public static APIManager _instance;

    private HttpRequest _httpRequest;

    private void Awake()
    {
        _instance = this;
        _httpRequest = new HttpRequest();
    }

    public async Task Login(string playerName, string password)
    {
        var loginRequest = new LoginRequest
        {
            playerName = playerName,
            password = password
        };

        var loginResponse = await _httpRequest.PostAsync<LoginResponse>("https://api.victorkrogh.dk/api/Auth/login", loginRequest);
        Debug.Log(loginResponse.accessToken);
    }
}