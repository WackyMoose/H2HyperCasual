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
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;

    private HttpRequest _httpRequest;

    private void Start()
    {
        _httpRequest = new HttpRequest();
        loginButton.onClick.AddListener(Login);
    }

    private async void Login()
    {
        var loginRequest = new LoginRequest
        {
            playerName = playerNameInput.text,
            password = passwordInput.text
        };

        var loginResponse = await _httpRequest.PostAsync<LoginResponse>("https://api.victorkrogh.dk/api/Auth/login", loginRequest);
        Debug.Log(loginResponse.accessToken);
    }
}