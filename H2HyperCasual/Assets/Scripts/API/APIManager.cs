using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
    private HttpRequest _httpRequest;

    private void Start()
    {
        Task.Run(async () =>
        {
            _httpRequest = new HttpRequest();
            var loginResponse = await _httpRequest.PostAsync<LoginResponse>("https://api.victorkrogh.dk/api/Auth/login", new LoginRequest { playerName = "kingoboiii", password = "test1234" });
            Debug.Log(loginResponse.accessToken);

            var player = await _httpRequest.GetAsync<Player>("https://api.victorkrogh.dk/api/Player/get-player?id=2", loginResponse.accessToken);
            Debug.Log($"Player name: {player.playerName}");
            Debug.Log(JsonUtility.ToJson(player));
        });
    }
}