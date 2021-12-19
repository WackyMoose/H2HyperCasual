using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;

    [SerializeField] private TMP_Text errorText;

    private PlayerManager _playerManager;

    private void Start()
    {
        errorText.enabled = false;
        _playerManager = PlayerManager.Instance;

        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(Register);
    }

    private async void Login()
    {
        errorText.enabled = false;
        var loginResponse = await APIManager.Instance.LoginAsync(playerNameInput.text, passwordInput.text);
        if (loginResponse.ContainsError)
        {
            errorText.enabled = true;
            errorText.text = loginResponse.ErrorMessage;
            return;
        }
        _playerManager.PopulatePlayerData(loginResponse.Data.accessToken, loginResponse.Data.player);
    }

    private async void Register()
    {
        errorText.enabled = false;
        var registerResponse = await APIManager.Instance.RegisterAsync(playerNameInput.text, passwordInput.text);
        if (registerResponse.ContainsError)
        {
            errorText.enabled = true;
            errorText.text = registerResponse.ErrorMessage;
            return;
        }
    }
}
